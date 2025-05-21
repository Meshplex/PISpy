import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button } from '@mui/material';
import PropTypes from 'prop-types';

const AddKeyDialog = ({ open, onClose, newString, setNewString, onSave }) => {
  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Schlüssel hinzufügen</DialogTitle>
      <DialogContent>
        <TextField
          label="Neuer Schlüssel"
          type="text"
          fullWidth
          sx={{ mt: 2 }}
          value={newString}
          onChange={(e) => setNewString(e.target.value)}
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
AddKeyDialog.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  newString: PropTypes.string.isRequired,
  setNewString: PropTypes.func.isRequired,
  onSave: PropTypes.func.isRequired,
};

export default AddKeyDialog;