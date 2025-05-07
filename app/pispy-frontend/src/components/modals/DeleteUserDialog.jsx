import PropTypes from 'prop-types';
import { Dialog, DialogTitle, DialogContent, DialogActions, Typography, Button } from '@mui/material';

const DeleteUserDialog = ({ open, onClose, onDelete }) => {
  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Konto löschen</DialogTitle>
      <DialogContent>
        <Typography>
          Bist du sicher, dass du diesen Benutzer löschen möchtest?
        </Typography>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Abbrechen</Button>
        <Button variant="contained" color="error" onClick={onDelete}>
          Löschen
        </Button>
      </DialogActions>
    </Dialog>
  );
};
DeleteUserDialog.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onDelete: PropTypes.func.isRequired,
};

export default DeleteUserDialog;
