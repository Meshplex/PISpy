import { useEffect, useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  List,
  ListItem,
  ListItemText,
  IconButton,
  CircularProgress,
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import PropTypes from 'prop-types';
import AddKeyDialog from './AddKeyDialog';

// Importiere die ausgelagerten API-Funktionen
import { fetchUserKeys, deleteUserKey, addUserKey } from '../../api/apiService';

const UserKeysDialog = ({ open, onClose, userId }) => {
  const [keys, setKeys] = useState([]);
  const [loading, setLoading] = useState(false);
  const [addKeyOpen, setAddKeyOpen] = useState(false);
  const [newKey, setNewKey] = useState('');

  useEffect(() => {
    if (open) {
      const loadKeys = async () => {
        setLoading(true);
        try {
          const data = await fetchUserKeys(userId);
          setKeys(data);
        } catch (error) {
          console.error('Error fetching keys:', error);
        } finally {
          setLoading(false);
        }
      };

      loadKeys();
    }
  }, [open, userId]);

  // Löscht einen Schlüssel per API-Call und aktualisiert danach die Liste
  const handleDeleteKey = async (keyId) => {
    try {
      await deleteUserKey(userId, keyId);
      // Aktualisiere die Schlüssel-Liste lokal, indem der gelöschte Schlüssel entfernt wird
      setKeys((prevKeys) => prevKeys.filter((key) => key.id !== keyId));
    } catch (error) {
      console.error('Error deleting key:', error);
    }
  };

  // Wird aufgerufen, wenn im AddKeyDialog "Speichern" geklickt wird
  const handleAddKeySave = async () => {
    try {
      const addedKey = await addUserKey(userId, newKey);
      // Füge den neuen Schlüssel der Liste hinzu
      setKeys((prevKeys) => [...prevKeys, addedKey]);
      setNewKey('');
      setAddKeyOpen(false);
    } catch (error) {
      console.error('Error adding key:', error);
    }
  };

  return (
    <>
      <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
        <DialogTitle>Deine Schlüssel</DialogTitle>
        <DialogContent>
          {loading ? (
            <CircularProgress />
          ) : (
            <List>
              {keys.map((key) => (
                <ListItem
                  key={key.id}
                  secondaryAction={
                    <IconButton
                      edge="end"
                      aria-label="delete"
                      onClick={() => handleDeleteKey(key.id)}
                    >
                      <DeleteIcon />
                    </IconButton>
                  }
                >
                  <ListItemText primary={key.value} />
                </ListItem>
              ))}
            </List>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Schließen</Button>
          <Button variant="contained" onClick={() => setAddKeyOpen(true)}>
            Neuen Schlüssel hinzufügen
          </Button>
        </DialogActions>
      </Dialog>

      <AddKeyDialog
        open={addKeyOpen}
        onClose={() => setAddKeyOpen(false)}
        newString={newKey}
        setNewString={setNewKey}
        onSave={handleAddKeySave}
      />
    </>
  );
};

UserKeysDialog.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  userId: PropTypes.string,
};

export default UserKeysDialog;
