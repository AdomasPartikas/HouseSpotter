import React from 'react';
import './assets/styles/Register.scss';



function Register() {
    const [formData, setFormData] = useState({
        username: '',
        password: '',
        email: '',
        phoneNumber: ''
    });

  const handleSubmit = (event) => {
    event.preventDefault();
  };

  return (
    <div className="register-page">
      <div className="register-container">
        <h2>Register</h2>
        <form className="register-form" onSubmit={handleSubmit}>
            <div className="form-group">
                <label htmlFor="username">Username</label>
                <input type="text" id="username" placeholder="Enter your username" />
            </div>
            <div className="form-group">
                <label htmlFor="password">Password</label>
                <input type="password" id="password" placeholder="Enter your password" />
            </div>
            <div className="form-group">
                <label htmlFor="confirm-password">Confirm Password</label>
                <input type="password" id="confirm-password" placeholder="Confirm your password" />
            </div>
            <div className="form-group">
                <label htmlFor="email">Email</label>
                <input type="email" id="email" placeholder="Enter your email" />
            </div>
            <div className="form-group">
                <label htmlFor="phone">Phone</label>
                <input type="tel" id="phone" placeholder="Enter your phone number" />
            </div>
            <button type="submit" className="register-button">Register</button>
        </form>
      </div>
    </div>
  );
}

export default Register;
