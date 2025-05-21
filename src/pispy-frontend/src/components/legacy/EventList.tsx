import PropTypes from 'prop-types';
import { List, ListItem, ListItemText, Typography } from '@mui/material';

const formatTimestamp = (isoString) => {
  const date = new Date(isoString);
  const day = date.getDate().toString().padStart(2, '0');
  const month = (date.getMonth() + 1).toString().padStart(2, '0');
  const year = date.getFullYear();
  const hours = date.getHours().toString().padStart(2, '0');
  const minutes = date.getMinutes().toString().padStart(2, '0');
  const seconds = date.getSeconds().toString().padStart(2, '0');
  return `${day}.${month}.${year} - ${hours}:${minutes}:${seconds}`;
};

const EventList = ({ events }) => {
  if (!Array.isArray(events) || events.length === 0) {
    return (
      <Typography variant="body2">
        Keine Daten oder Verbindung noch nicht aktiv.
      </Typography>
    );
  }
  
  return (
    <List>
      {events.map((item, index) => (
        <ListItem key={index}>
          <ListItemText
            primary={`${item.description} ,ID: ${item.eventId} ,Schlüssel: ${item.keyId}, Nutzer: ${item.userId}, Uhrzeit: ${formatTimestamp(item.timestamp)}`}
          />
        </ListItem>
      ))}
    </List>
  );
};
EventList.propTypes = {
  events: PropTypes.arrayOf(
    PropTypes.shape({
      description: PropTypes.string.isRequired,
      eventId: PropTypes.number.isRequired,
      keyId: PropTypes.string.isRequired,
      userId: PropTypes.number.isRequired,
      timestamp: PropTypes.string.isRequired,
    })
  ).isRequired,
};

export default EventList;
