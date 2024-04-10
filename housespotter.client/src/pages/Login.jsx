import React, { useState } from "react";
import Header from "../components/header";
import { Link, useNavigate } from "react-router-dom";
import TextInput from "../components/form/TextInput";
import { useAuth } from "../contexts/AuthContext";
import { useNotification } from '../contexts/NotificationContext';

function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const { login } = useAuth();
  const { notify } = useNotification();
  const navigate = useNavigate();

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    if (name === "text") setUsername(value);
    else if (name === "password") setPassword(value);
  };

  async function loginUser() {
    const payload = {
      username: username,
      password: password,
    };

    try {
      const response = await fetch("housespotter/db/user/login", {
        method: "POST",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      });

      if (response.ok) {
        const data = await response.json();
        login(data);
        notify(`Sveiki prisijungę, ${data.username}!`, 'success');
        navigate("/megstamiausi");
      }
    } catch (error) {
      console.log(error);
      notify('Prisijungti nepavyko.', 'error');
    }
  }

  return (
    <div className="login">
      <Header />
      <div className="hero">
        <div className="layout">
          <form
            onSubmit={(event) => {
              event.preventDefault();
              loginUser();
            }}
          >
            <h1>Prisijungti</h1>
            <TextInput
              label="Vartotojo vardas"
              name="text"
              placeholder="Įveskite savo vartotojo vardą"
              inputType="text"
              value={username}
              onChange={handleInputChange}
            />
            <TextInput
              label="Slaptažodis"
              name="password"
              placeholder="Įveskite savo slaptažodį"
              inputType="password"
              value={password}
              onChange={handleInputChange}
            />
            <button type="submit" className="primary__btn">
              Prisijungti
            </button>
            <p>Neturite paskyros?</p>
            <Link to="/registruotis" className="secondary__btn">
              Registruotis
            </Link>
          </form>
        </div>
      </div>
    </div>
  );
}

export default Login;
