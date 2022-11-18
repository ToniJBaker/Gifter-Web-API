import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { register } from "../modules/UserManager";

export const Register = ({setIsLoggedIn}) => {
  // Create state variables for each form field
  const [email, setEmail] = useState("");
  const [name, setName] = useState("");
  const [bio, setBio] = useState("");
  const [imageurl, setImageurl] = useState("");


  const navigate = useNavigate()
  
  // This function will run when the user has finished filling out the form and clicks submit
  const submitLoginForm = (e) => {
      e.preventDefault();
  const newUser = {
  name: name,
  email: email,
  bio: bio,
  imageurl: imageurl
  }
    register(newUser)
    .then(() => {
        setIsLoggedIn(true)
        navigate("/")

    });
  };

  return (
    <>
    <h2>Register</h2>
      <form>
        <input
          type="text"
          onChange={(e) => setEmail(e.target.value)}
          placeholder="email"
        />
        <input
          type="text"
          onChange={(e) => setName(e.target.value)}
          placeholder="name"
        />
        <input
          type="text"
          onChange={(e) => setBio(e.target.value)}
          placeholder="bio"
        />{" "}
        <input
          type="text"
          onChange={(e) => setImageurl(e.target.value)}
          placeholder="imageurl"
        />
        <button type="submit" onClick={submitLoginForm}>
          Register
        </button>
      </form>
    </>
  );
};
