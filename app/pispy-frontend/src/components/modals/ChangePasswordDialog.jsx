import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button } from '@mui/material';
import PropTypes from 'prop-types';

const ChangePasswordDialog = ({ open, onClose, newPassword, setNewPassword, onSave }) => {
  return (
    <Dialog open={open} onClose={onClose}>
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
        <Button onClick={onClose}>Abbrechen</Button>
        <Button variant="contained" onClick={onSave}>
          Speichern
        </Button>
      </DialogActions>
    </Dialog>
  );
};
ChangePasswordDialog.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  newPassword: PropTypes.string.isRequired,
  setNewPassword: PropTypes.func.isRequired,
  onSave: PropTypes.func.isRequired,
};

export default ChangePasswordDialog;
