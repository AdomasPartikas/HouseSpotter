import React, { useState } from 'react';
import { useNotification } from './contexts/NotificationContext';
import api from './net/api';
import './assets/styles/Register.scss';

function Register() {
  const { notify } = useNotification();

  const [formData, setFormData] = useState({
    username: '',
    password: '',
    confirmPassword: '',
    email: '',
    phoneNumber: ''
  });

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData(prev => ({
        ...prev,
        [name]: value
    }));
};

const handleSubmit = async (event) => {
  event.preventDefault();
  if (formData.password !== formData.confirmPassword) {
      notify('Passwords do not match', 'error');
      return;
  }

  try {
      const response = await api.post('/housespotter/db/user/register', {
          username: formData.username,
          password: formData.password,
          email: formData.email,
          phoneNumber: formData.phoneNumber,
          isAdmin: false
      });
      notify('Registration successful', 'success');
      setFormData({ username: '', password: '', confirmPassword: '', email: '', phoneNumber: '' });
  } catch (error) {
      notify(error.response?.data?.message || 'Registration failed', 'error');
  }
};

return (
  <div className="register-page">
      <div className="register-container">
          <h2>Register</h2>
          <form className="register-form" onSubmit={handleSubmit}>
              <div className="form-group">
                  <label htmlFor="username">Username</label>
                  <input type="text" name="username" id="username" value={formData.username} onChange={handleChange} placeholder="Enter your username" required />
              </div>
              <div className="form-group">
                  <label htmlFor="password">Password</label>
                  <input type="password" name="password" id="password" value={formData.password} onChange={handleChange} placeholder="Enter your password" required />
              </div>
              <div className="form-group">
                  <label htmlFor="confirmPassword">Confirm Password</label>
                  <input type="password" name="confirmPassword" id="confirmPassword" value={formData.confirmPassword} onChange={handleChange} placeholder="Confirm your password" required />
              </div>
              <div className="form-group">
                  <label htmlFor="email">Email</label>
                  <input type="email" name="email" id="email" value={formData.email} onChange={handleChange} placeholder="Enter your email" required />
              </div>
              <div className="form-group">
                  <label htmlFor="phone">Phone</label>
                  <input type="tel" name="phoneNumber" id="phone" value={formData.phoneNumber} onChange={handleChange} placeholder="Enter your phone number" />
              </div>
              <button type="submit" className="register-button">Register</button>
          </form>
      </div>
  </div>
);
}

export default Register;
