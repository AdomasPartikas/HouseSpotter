import React, { useState } from 'react';
import './assets/styles/Login.scss';
import Cookies from 'js-cookie';
import api from './net/api';
import { useNotification } from './contexts/NotificationContext';

function Login({ onRegister, onLoginSuccess }) {
    const { notify } = useNotification();
    const [credentials, setCredentials] = useState({
        username: '',
        password: '',
    });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setCredentials(prevCredentials => ({
            ...prevCredentials,
            [name]: value
        }));
    };

    const handleLogin = async (event) => {
        event.preventDefault();
        try {
            const response = await api.post('/housespotter/db/user/login', credentials);
            if (response.data) {
                Cookies.set('jwt', response.data.token, { expires: 1 });
                Cookies.set('username', response.data.username, { expires: 1 });
                onLoginSuccess(response.data.username);
                notify('Login successful', 'success');
            }
        } catch (error) {
            console.error('Login failed:', error);
            notify('Login failed. Please try again.', 'error');
        }
    };


    return (
        <div className="login-page">
            <div className="login-container">
                <h2>Login</h2>
                <form onSubmit={handleLogin} className="login-form">
                    <div className="form-group">
                        <label htmlFor="username">Username</label>
                        <input
                            type="text"
                            name="username"
                            value={credentials.username}
                            onChange={handleInputChange}
                            placeholder="Enter your username"
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="password">Password</label>
                        <input
                            type="password"
                            name="password"
                            value={credentials.password}
                            onChange={handleInputChange}
                            placeholder="Enter your password"
                            required
                        />
                    </div>
                    <button type="submit" className="login-button">Login</button>
                    <button type="button" onClick={onRegister} className="register-button">I don't have an account</button>
                </form>
            </div>
        </div>
    );
}

export default Login;
