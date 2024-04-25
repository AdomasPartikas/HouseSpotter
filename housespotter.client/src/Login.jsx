import React, { useState } from 'react';
import axios from 'axios';
import './assets/styles/Login.scss';

function Login({ onRegister, onLoginSuccess }) {
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
            const response = await axios.post('/housespotter/db/user/login', credentials);
            if (response.data) {
                onLoginSuccess(response.data.username);
            }
        } catch (error) {
            console.error('Login failed:', error);
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
