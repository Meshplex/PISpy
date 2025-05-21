import { Paper, Typography, Switch } from '@mui/material';
import PropTypes from 'prop-types';

const AlarmPanel = ({ alarmArmed, onToggle }) => {
  return (
    <Paper sx={{ p: 2 }}>
      <Typography variant="h6">Alarmanlage</Typography>
      <Typography variant="body1">
        Status: {alarmArmed ? 'Scharf' : 'Nicht scharf'}
      </Typography>
      <Switch
        checked={alarmArmed}
        onChange={onToggle}
        color="primary"
      />
    </Paper>
  );
};
AlarmPanel.propTypes = {
  alarmArmed: PropTypes.bool.isRequired,
  onToggle: PropTypes.func.isRequired,
};

export default AlarmPanel;
