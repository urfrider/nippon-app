import React from "react";
import styled from "styled-components";
import { useForm } from "react-hook-form";
import { Card, Form } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { authAtom } from "../../atoms";
import { useSetRecoilState } from "recoil";
import Logo from "../../static/images/login-logo.png";

const Wrapper = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  .login {
    width: 100%;
    background-color: transparent;
    border-radius: 7px;
    height: 40px;
    border: 1px solid blue;
    color: #0d6efd;
    &:hover {
      background-color: #0d6efd;
      color: white;
      transition: ease-in-out 0.3s;
    }
  }
`;
const Error = styled.p`
  color: red;
  margin-top: 5px;
`;

function Login() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();
  const navigate = useNavigate();
  const setAuth = useSetRecoilState(authAtom);

  // handle login and store the token
  const handleValid = async ({ username, password }) => {
    const data = { username: username, password: password };
    console.log(data);
    try {
      const response = await axios.post(
        "https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/login",
        data,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      localStorage.setItem("user", JSON.stringify(response));
      setAuth(response);
    } catch (error) {
      console.log(error);
    } finally {
      navigate("/dashboard");
    }
  };

  return (
    <div
      style={{
        display: "flex",
        width: "100%",
        height: "100%",
      }}
    >
      <div
        style={{
          height: "100vh",
          backgroundColor: "#bdbdbd",
          flex: "1",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          width: "50%",
        }}
      >
        <img
          style={{
            objectFit: "cover",
            width: "100%",
            flexShrink: "0",
            minWidth: "100%",
            minHeight: "100%",
          }}
          src={require("../../static/images/login.jpg")}
          alt="not found"
        />
      </div>
      <div
        style={{
          height: "100vh",
          backgroundColor: "white",
          color: "black",
          flex: "1",
          display: "flex",
          justifyContent: "center",
          alignItems: " center",
        }}
      >
        <Card
          className="p-2"
          style={{
            minWidth: "400px",
          }}
        >
          <Card.Img
            variant="top"
            src={Logo}
            className="mx-auto d-flex justify-content-center py-3"
            style={{
              height: "auto",
              width: "300px",
            }}
          />
          <Card.Body>
            <form id="loginForm" onSubmit={handleSubmit(handleValid)}>
              <Form.Group className="mb-3" controlId="usernameInput">
                <Form.Label>Username</Form.Label>
                <Form.Control
                  type="text"
                  {...register("username", {
                    required: "This field is required",
                    maxLength: {
                      value: 50,
                      message: "Max length is 50 char.",
                    },
                  })}
                />
                <Error>{errors?.username?.message}</Error>
              </Form.Group>
              <Form.Group className="mb-3" controlId="passwordInput">
                <Form.Label>Password</Form.Label>
                <Form.Control
                  type="password"
                  {...register("password", {
                    required: "This field is required",
                    maxLength: {
                      value: 20,
                      message: "Max length is 20 char.",
                    },
                  })}
                />
                <Error>{errors?.password?.message}</Error>
              </Form.Group>
              <Wrapper>
                <button className="login">Login</button>
              </Wrapper>
            </form>
          </Card.Body>
        </Card>
      </div>
    </div>
  );
}

export default Login;
