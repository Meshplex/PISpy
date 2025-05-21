import { List, ListItem, ListItemText, IconButton, Typography } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import NoteAddIcon from '@mui/icons-material/NoteAdd';
import PropTypes from 'prop-types';

const UserList = ({ users, onChangePassword, onDeleteUser, onUpdateString }) => {
  if (!users || users.length === 0) {
    return (
      <Typography variant="body2">
        Keine Benutzer gefunden.
      </Typography>
    );
  }
  return (
    <List>
      {users.map((user) => (
        <ListItem
          key={user.id}
          secondaryAction={
            <>
              <IconButton edge="end" aria-label="edit" onClick={() => onChangePassword(user.id)}>
                <EditIcon />
              </IconButton>
              <IconButton edge="end" aria-label="delete" onClick={() => onDeleteUser(user.id)}>
                <DeleteIcon />
              </IconButton>
              <IconButton edge="end" aria-label="update-string" onClick={() => onUpdateString(user.id)}>
                <NoteAddIcon />
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
  );
};
UserList.propTypes = {
  users: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      username: PropTypes.string,
    })
  ).isRequired,
  onChangePassword: PropTypes.func.isRequired,
  onDeleteUser: PropTypes.func.isRequired,
  onUpdateString: PropTypes.func.isRequired,
};

export default UserList;
