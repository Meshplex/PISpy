import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  TextField,
  Button,
  Typography,
  Container,
  Alert,
  Box,
} from '@mui/material';
import { API_BASE } from '../config';

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      console.log(API_BASE);
      const response = await fetch(`/api/User/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password }),
      });

      if (response.ok) {
        const result = await response.json();
        const jwt = result.token;
        localStorage.setItem("token", jwt);
        navigate("/dashboard");
      } else {
        setErrorMessage("Login fehlgeschlagen. Bitte prüfe deine Daten.");
      }
    } catch (error) {
      console.error(error);
      setErrorMessage("Serverfehler oder Netzwerkproblem.");
    }
  };

  return (
    <Container
      maxWidth={false}            
      disableGutters             
      sx={{
        minHeight: '100vh',      
        display: 'flex',         
        alignItems: 'center',    
        justifyContent: 'center',
        backgroundColor: '#f0f0f0',
      }}
    >
      <Box
        component="form"
        onSubmit={handleSubmit}
        sx={{
          width: '90%',            
          maxWidth: 400,           
          backgroundColor: 'white',
          p: 4,                   
          borderRadius: 2,         
          boxShadow: 3,            
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <Typography component="h1" variant="h5">
          Login
        </Typography>

        {errorMessage && (
          <Alert severity="error" sx={{ mt: 2 }}>
            {errorMessage}
          </Alert>
        )}

        <TextField
          margin="normal"
          required
          fullWidth
          id="username"
          label="Benutzername"
          name="username"
          autoComplete="username"
          autoFocus
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />

        <TextField
          margin="normal"
          required
          fullWidth
          name="password"
          label="Passwort"
          type="password"
          id="password"
          autoComplete="current-password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />

        <Button
          type="submit"
          fullWidth
          variant="contained"
          sx={{ mt: 3 }}
        >
          Einloggen
        </Button>
      </Box>
    </Container>
  );
};

export default Login;
