import React, { useState } from "react";
import Header from "../components/header";
import { Link, useNavigate } from "react-router-dom";
import TextInput from "../components/form/TextInput";
import { useNotification } from "../contexts/NotificationContext";

function Register() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [username, setUsername] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const navigate = useNavigate();
  const { notify } = useNotification();

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    switch (name) {
      case "email":
        setEmail(value);
        break;
      case "password":
        setPassword(value);
        break;
      case "username":
        setUsername(value);
        break;
      case "phone":
        setPhoneNumber(value);
        break;
      default:
        break;
    }
  };

  async function registerUser() {
    const payload = {
      email: email,
      phoneNumber: phoneNumber,
      username: username,
      password: password,
      isAdmin: false,
    };

    try {
      const response = await fetch("housespotter/db/user/register", {
        method: "POST",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      });

      if (response.ok) {
        notify(
          `Sveikiname sėkmingai prisiregistravus. Dabar galite prisijungti.`,
          "success"
        );
        navigate("/prisijungti");
      } else {
        notify("Registracija nepavyko.", "error");
      }
    } catch (error) {
      notify("Registracija nepavyko.", "error");
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
              registerUser();
            }}
          >
            <h1>Registruotis</h1>
            <TextInput
              label="Vartotojo vardas"
              name="username"
              placeholder="Įveskite savo vartotojo vardą"
              inputType="text"
              onChange={handleInputChange}
            />
            <TextInput
              label="El. paštas"
              name="email"
              placeholder="Įveskite savo el. paštą"
              inputType="email"
              onChange={handleInputChange}
            />
            <TextInput
              label="Telefonas"
              name="phone"
              placeholder="Įveskite savo tel. nr."
              inputType="text"
              onChange={handleInputChange}
            />
            <TextInput
              label="Slaptažodis"
              name="password"
              placeholder="Įveskite savo slaptažodį"
              inputType="password"
              onChange={handleInputChange}
            />
            <button type="submit" className="primary__btn">
              Registruotis
            </button>
            <p>Jau turite paskyrą?</p>
            <Link to="/prisijungti" className="secondary__btn">
              Prisijungti
            </Link>
          </form>
        </div>
      </div>
    </div>
  );
}

export default Register;