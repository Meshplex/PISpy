// src/Dashboard.js
import { useEffect, useState, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import * as signalR from '@microsoft/signalr';
import LogoutIcon from '@mui/icons-material/Logout';
import { Grid, Paper, Typography, Button, Alert } from '@mui/material';

import EventList from '../components/EventList';
import AlarmPanel from '../components/AlarmPanel';
import UserList from '../components/UserList';
import ChangePasswordDialog from '../components/modals/ChangePasswordDialog';
import DeleteUserDialog from '../components/modals/DeleteUserDialog';
import KeyDialog from '../components/modals/UserKeysDialog';

import {
  fetchEvents,
  fetchUsers,
  changeUserPassword,
  deleteUser,
  addUserKey,
  setAlarmStatus,
} from '../api/apiService';

const Dashboard = () => {
  const navigate = useNavigate();
  const [myList, setMyList] = useState([]);
  const [alarmArmed, setAlarmArmed] = useState(false);
  const [users, setUsers] = useState([]);
  const [error, setError] = useState('');
  const [openPasswordModal, setOpenPasswordModal] = useState(false);
  const [openDeleteModal, setOpenDeleteModal] = useState(false);
  const [openStringModal, setOpenStringModal] = useState(false);
  const [selectedUserId, setSelectedUserId] = useState(null);
  const [newPassword, setNewPassword] = useState('');
  const [newString, setNewString] = useState('');

  const isMountedRef = useRef(true);

  const updateMyList = (newEvents) => {
    setMyList(newEvents);
    localStorage.setItem('myList', JSON.stringify(newEvents));
  };

  // Lade Events aus dem lokalen Speicher
  useEffect(() => {
    const storedEvents = localStorage.getItem('myList');
    if (storedEvents) {
      setMyList(JSON.parse(storedEvents));
    }
  }, []);

  // SignalR-Verbindung initialisieren (bleibt hier)
  const connectionRef = useRef(
    new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5272/alarmHub')
      .withAutomaticReconnect()
      .build()
  );
  const connection = connectionRef.current;

  useEffect(() => {
    isMountedRef.current = true;

    const startConnection = async () => {
      if (connection.state === signalR.HubConnectionState.Disconnected) {
        try {
          await connection.start();
          if (!isMountedRef.current) return;
          console.log('SignalR connected.');
          connection.on('NewEvents', (data) => {
            updateMyList(data);
          });
          connection.on('ReceivePropertyStatus', (status) => {
            setAlarmArmed(status);
          });
        } catch (err) {
          if (err.message && err.message.includes('stopped during negotiation')) {
            console.warn('Connection start aborted during negotiation:', err);
          } else {
            console.error('Connection failed:', err);
          }
        }
      }
    };

    startConnection();

    return () => {
      isMountedRef.current = false;
      if (connection.state === signalR.HubConnectionState.Connected) {
        connection.stop().catch((err) => {
          if (err.message && !err.message.includes('stopped during negotiation')) {
            console.error('Error while stopping connection: ', err);
          }
        });
      }
    };
  }, [connection]);

  // Initiale Daten laden
  useEffect(() => {
    (async () => {
      try {
        const events = await fetchEvents();
        updateMyList(events);
      } catch (err) {
        console.error(err);
        setError(err.message);
      }
    })();

    (async () => {
      try {
        const usersData = await fetchUsers();
        setUsers(usersData);
      } catch (err) {
        console.error(err);
        setError(err.message);
      }
    })();
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/');
  };

  const handleAlarmToggle = async (event) => {
    const newStatus = event.target.checked;
    setAlarmArmed(newStatus);
    try {
      await setAlarmStatus(newStatus);
    } catch (err) {
      console.error(err);
      setError(err.message);
    }
  };

  // Passwort ändern
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
      await changeUserPassword(selectedUserId, newPassword);
      setOpenPasswordModal(false);
    } catch (err) {
      console.error(err);
      setError(err.message);
    }
  };

  // Benutzer löschen
  const handleOpenDeleteModal = (userId) => {
    setSelectedUserId(userId);
    setOpenDeleteModal(true);
  };

  const handleCloseDeleteModal = () => {
    setOpenDeleteModal(false);
  };

  const handleDeleteUser = async () => {
    try {
      await deleteUser(selectedUserId);
      setOpenDeleteModal(false);
      setUsers(users.filter((u) => u.id !== selectedUserId));
    } catch (err) {
      console.error(err);
      setError(err.message);
    }
  };

  // String aktualisieren
  const handleOpenStringModal = (userId) => {
    setSelectedUserId(userId);
    setNewString('');
    setOpenStringModal(true);
  };

  const handleCloseStringModal = () => {
    setOpenStringModal(false);
  };

  const handleSubmitStringChange = async () => {
    try {
      await addUserKey(selectedUserId, newString);
      setOpenStringModal(false);
    } catch (err) {
      console.error(err);
      setError(err.message);
    }
  };

  return (
    <Grid container spacing={2} sx={{ p: 2 }}>
      {/* Logout-Button */}
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

      {/* Events */}
      <Grid item xs={12}>
        <Paper sx={{ p: 2 }}>
          <Typography variant="h6">Events</Typography>
          <EventList events={myList} />
        </Paper>
      </Grid>

      {/* Alarmsteuerung */}
      <Grid item xs={12} md={6}>
        <AlarmPanel alarmArmed={alarmArmed} onToggle={handleAlarmToggle} />
      </Grid>

      {/* Benutzerkonten */}
      <Grid item xs={12} md={6}>
        <Paper sx={{ p: 2 }}>
          <Typography variant="h6">Benutzerkonten</Typography>
          {error && <Alert severity="error">{error}</Alert>}
          <UserList
            users={users}
            onChangePassword={handleOpenChangePassword}
            onDeleteUser={handleOpenDeleteModal}
            onUpdateString={handleOpenStringModal}
          />
        </Paper>
      </Grid>

      {/* Modals */}
      <ChangePasswordDialog
        open={openPasswordModal}
        onClose={handleCloseChangePassword}
        newPassword={newPassword}
        setNewPassword={setNewPassword}
        onSave={handleChangePassword}
      />

      <DeleteUserDialog
        open={openDeleteModal}
        onClose={handleCloseDeleteModal}
        onDelete={handleDeleteUser}
      />

      <KeyDialog
        open={openStringModal}
        onClose={handleCloseStringModal}
        newString={newString}
        setNewString={setNewString}
        onSave={handleSubmitStringChange}
      />
    </Grid>
  );
};

export default Dashboard;
