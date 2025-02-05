import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import * as signalR from '@microsoft/signalr';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import LogoutIcon from '@mui/icons-material/Logout';

import {
  Grid,
  Paper,
  Typography,
  Button,
  Switch,
  List,
  ListItem,
  ListItemText,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Alert,
} from '@mui/material';

const Dashboard = () => {
  const navigate = useNavigate();

  // SignalR connection
  const [connection, setConnection] = useState(null);

  // --------------------
  // 1) Daten-Liste (SignalR)
  // --------------------
  const [myList, setMyList] = useState([]);

  // --------------------
  // 2) Alarm-Status (SignalR) + API-Update
  // --------------------
  const [alarmArmed, setAlarmArmed] = useState(false);

  // --------------------
  // 3) User-Liste + Modals
  // --------------------
  const [users, setUsers] = useState([]);
  const [error, setError] = useState('');

  // A) Passwort ändern - Modal
  const [openPasswordModal, setOpenPasswordModal] = useState(false);
  const [selectedUserId, setSelectedUserId] = useState(null);
  const [newPassword, setNewPassword] = useState('');

  // B) Benutzer löschen - Bestätigungs-Dialog
  const [openDeleteModal, setOpenDeleteModal] = useState(false);

  // --------------------
  // Effekt: SignalR-Connection aufbauen
  // --------------------
  useEffect(() => {
    // SignalR-Verbindung initialisieren
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5272/alarmHub') // <--- Anpassen!
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log('SignalR connected.');

          // Beispiel: Empfange eine Liste über ein Event 'ReceiveList'
          connection.on('ReceiveList', (data) => {
            setMyList(data);
          });

          // Beispiel: Empfange Alarm-Status über 'ReceiveAlarmStatus'
          connection.on('ReceiveAlarmStatus', (status) => {
            setAlarmArmed(status);
          });

          // Falls du die User-Liste ebenso über SignalR aktualisieren willst:
          connection.on('ReceiveUserList', (userList) => {
            setUsers(userList);
          });
        })
        .catch((error) => console.error('Connection failed: ', error));
    }
  }, [connection]);

  // --------------------
  // (optional) Userliste initial per API laden
  // --------------------
  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      const res = await fetch('http://localhost:5272/api/User/all', {
        headers: { Authorization: 'Bearer ' + localStorage.getItem('token') },
      });
      if (!res.ok) throw new Error('Fehler beim Laden der User-Liste');
      const data = await res.json();
      setUsers(data);
    } catch (err) {
      console.error(err);
      setError(err.message);
    }
  };

  // --------------------
  // Logout
  // --------------------
  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/'); // Zurück zur Login-Seite (anpassen nach Bedarf)
  };

  // --------------------
  // Alarmstatus per API ändern
  // --------------------
  const handleAlarmToggle = async (event) => {
    const newStatus = event.target.checked;
    setAlarmArmed(newStatus);

    try {
      // API-Call zum Setzen des neuen Status
      const res = await fetch('http://localhost:5272/api/Alarm/SetStatus', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: 'Bearer ' + localStorage.getItem('token'),
        },
        body: JSON.stringify({ isArmed: newStatus }),
      });
      if (!res.ok) {
        throw new Error('Konnte Alarmstatus nicht ändern.');
      }
      // optional: Erfolgsmeldung / State-Update
    } catch (err) {
      console.error(err);
      setError(err.message);
    }
  };

  // --------------------
  // Passwort ändern - Dialog öffnen & Funktion
  // --------------------
  const handleOpenChangePassword = (userId) => {
    setSelectedUserId(userId);
    setNewPassword('');
    setOpenPasswordModal(true);
  };

  const handleCloseChangePassword = () => {
    setOpenPasswordModal(false);
  };

  const handleChangePassword = async () => {
    try {
      const res = await fetch('http://localhost:5272/api/User/changePassword', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: 'Bearer ' + localStorage.getItem('token'),
        },
        body: JSON.stringify({
          userId: selectedUserId,
          newPassword,
        }),
      });
      if (!res.ok) {
        throw new Error('Passwort konnte nicht geändert werden.');
      }
      // Modal schließen und ggf. Erfolgsmeldung anzeigen
      setOpenPasswordModal(false);
    } catch (err) {
      console.error(err);
      setError(err.message);
    }
  };

  // --------------------
  // Benutzer löschen - Dialog öffnen & Funktion
  // --------------------
  const handleOpenDeleteModal = (userId) => {
    setSelectedUserId(userId);
    setOpenDeleteModal(true);
  };

  const handleCloseDeleteModal = () => {
    setOpenDeleteModal(false);
  };

  const handleDeleteUser = async () => {
    try {
      const res = await fetch(
        `http://localhost:5272/api/User/delete/${selectedUserId}`,
        {
          method: 'DELETE',
          headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token'),
          },
        }
      );
      if (!res.ok) {
        throw new Error('Benutzer konnte nicht gelöscht werden.');
      }
      // Modal schließen und lokale Userliste aktualisieren
      setOpenDeleteModal(false);
      setUsers(users.filter((u) => u.id !== selectedUserId));
    } catch (err) {
      console.error(err);
      setError(err.message);
    }
  };

  return (
    <Grid container spacing={2} sx={{ p: 2 }}>
      {/* Kopfzeile mit Logout-Button */}
      <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'flex-end' }}>
        <Button
          variant="outlined"
          color="error"
          startIcon={<LogoutIcon />}
          onClick={handleLogout}
        >
          Logout
        </Button>
      </Grid>

      {/* Erste Zeile: Liste aus SignalR (myList) */}
      <Grid item xs={12}>
        <Paper sx={{ p: 2 }}>
          <Typography variant="h6">SignalR-Daten (nicht sortierbar)</Typography>
          {myList.length === 0 && (
            <Typography variant="body2">
              Keine Daten oder Verbindung noch nicht aktiv.
            </Typography>
          )}
          <List>
            {myList.map((item, index) => (
              <ListItem key={index}>
                <ListItemText primary={item} />
              </ListItem>
            ))}
          </List>
        </Paper>
      </Grid>

      {/* Zweite Zeile: Links der Switch (Alarmstatus), rechts die Userliste */}
      <Grid item xs={12} md={6}>
        <Paper sx={{ p: 2 }}>
          <Typography variant="h6">Alarmanlage</Typography>
          <Typography variant="body1">
            Status: {alarmArmed ? 'Scharf' : 'Nicht scharf'}
          </Typography>
          <Switch
            checked={alarmArmed}
            onChange={handleAlarmToggle}
            color="primary"
          />
        </Paper>
      </Grid>

      <Grid item xs={12} md={6}>
        <Paper sx={{ p: 2 }}>
          <Typography variant="h6">Benutzerkonten</Typography>

          {error && <Alert severity="error">{error}</Alert>}
          {users.length === 0 && (
            <Typography variant="body2">Keine Benutzer gefunden.</Typography>
          )}
          <List>
            {users.map((user) => (
              <ListItem
                key={user.id}
                secondaryAction={
                  <>
                    <IconButton
                      edge="end"
                      aria-label="edit"
                      onClick={() => handleOpenChangePassword(user.id)}
                    >
                      <EditIcon />
                    </IconButton>
                    <IconButton
                      edge="end"
                      aria-label="delete"
                      onClick={() => handleOpenDeleteModal(user.id)}
                    >
                      <DeleteIcon />
                    </IconButton>
                  </>
                }
              >
                <ListItemText
                  primary={user.username}
                  secondary={`ID: ${user.id}`}
                />
              </ListItem>
            ))}
          </List>
        </Paper>
      </Grid>

      {/* Dialog: Passwort ändern */}
      <Dialog open={openPasswordModal} onClose={handleCloseChangePassword}>
        <DialogTitle>Passwort ändern</DialogTitle>
        <DialogContent>
          <TextField
            label="Neues Passwort"
            type="password"
            fullWidth
            sx={{ mt: 2 }}
            value={newPassword}
            onChange={(e) => setNewPassword(e.target.value)}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseChangePassword}>Abbrechen</Button>
          <Button variant="contained" onClick={handleChangePassword}>
            Speichern
          </Button>
        </DialogActions>
      </Dialog>

      {/* Dialog: Benutzer löschen */}
      <Dialog open={openDeleteModal} onClose={handleCloseDeleteModal}>
        <DialogTitle>Konto löschen</DialogTitle>
        <DialogContent>
          <Typography>
            Bist du sicher, dass du diesen Benutzer löschen möchtest?
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDeleteModal}>Abbrechen</Button>
          <Button variant="contained" color="error" onClick={handleDeleteUser}>
            Löschen
          </Button>
        </DialogActions>
      </Dialog>
    </Grid>
  );
};

export default Dashboard;
