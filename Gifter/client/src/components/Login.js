import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { login } from "../modules/UserManager";


export const Login = ({setIsLoggedIn}) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");


  const navigate = useNavigate();
  
  const submitLoginForm = (e) => {
      e.preventDefault();
  const newLogin = {
   email: email,
   password: password
  }
    login(newLogin)
    .then(() => {
        setIsLoggedIn(true)
        navigate("/")

    });
    
  };

  return (
    <>
      <>
        <h2>Log In</h2>
        <form>
          <input
            type="text"
            onChange={(e) => setEmail(e.target.value)}
            name="email"
          />
          <input
            type="password"
            onChange={(e) => setPassword(e.target.value)}
            name="password"
          />
          <button type="submit" onClick={submitLoginForm}>
            Log In
          </button>
          <section className="link--register">
                <Link to="/register">Register New Guest</Link>
            </section>
        </form>
      </>
    </>
  );
};
