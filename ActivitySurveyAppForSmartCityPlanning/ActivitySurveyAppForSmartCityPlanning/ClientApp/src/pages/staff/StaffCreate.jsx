import React from "react";
import Sidebar from "../../components/Sidebar";
import styled from "styled-components";
import Navbar from "../../components/Navbar";
import { useForm } from "react-hook-form";
import Radio from "@mui/material/Radio";
import RadioGroup from "@mui/material/RadioGroup";
import FormControlLabel from "@mui/material/FormControlLabel";
import FormLabel from "@mui/material/FormLabel";
import axios from "axios";
import { authAtom } from "../../atoms";
import { useNavigate } from "react-router-dom";
import { useRecoilValue } from "recoil";
import {
  Container,
  MainContainer,
  DatatableWrapper,
  Header,
} from "../../components/Styled-Containers";

const Form = styled.form`
  width: 60%;
  min-height: 300px;
  margin: 50px 0;
  background-color: ${(props) => props.theme.bgColor};
  box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -webkit-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -moz-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  border-radius: 30px;
  padding: 2rem;
  color: black;
  .wrapper {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    margin-top: 20px;
    gap: 10px;
    .wrapper2 {
      width: 50%;
      display: flex;
      justify-content: flex-start;
    }
    .error {
      color: red;
      margin-top: 5px;
    }
  }
  input {
    width: 50%;
    height: 2rem;
  }
  .createBtn {
    margin-top: 50px;
    display: flex;
    justify-content: center;
    align-items: center;
    button {
      padding: 15px;
      border-radius: 10px;
      width: 30%;
      background-color: ${(props) => props.theme.accentColor};
      border: none;
      color: white;
      &:hover {
        background-color: #fdcb6e;
        transition: ease-in-out 0.2s;
      }
    }
  }
`;

const Title = styled.h1`
  color: ${(props) => props.theme.accentColor};
  font-weight: 600;
  text-align: center;
  margin-top: 50px;
  font-size: 2rem;
`;

const FormContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
`;

function StaffCreate() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();
  const auth = useRecoilValue(authAtom);
  const navigate = useNavigate();

  // on create staff button clicked
  const handleValid = async (data) => {
    try {
      await axios.post(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Registration/DashboardUser`,
        data,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      navigate("/staff");
    } catch (Error) {
      console.log(Error);
    }
  };
  return (
    <Container>
      <Sidebar />
      <MainContainer>
        <Navbar />
        <Title>DASHBOARD USER REGISTRATION</Title>
        <FormContainer>
          <Form onSubmit={handleSubmit(handleValid)}>
            <div className="wrapper">
              <div className="wrapper2">
                <span>First Name </span>
              </div>
              <input
                {...register("firstName", {
                  required: "This field is required",
                  maxLength: {
                    value: 50,
                    message: "Max length is 50 char.",
                  },
                })}
                placeholder="First name"
              />
              <p className="error">{errors?.firstName?.message}</p>
            </div>
            <div className="wrapper">
              <div className="wrapper2">
                <span>Last Name </span>
              </div>
              <input
                {...register("lastName", {
                  required: "This field is required",
                  maxLength: {
                    value: 50,
                    message: "Max length is 50 char.",
                  },
                })}
                placeholder="Last name"
              />
              <p className="error">{errors?.lastName?.message}</p>
            </div>
            <div className="wrapper">
              <div className="wrapper2">
                <div>
                  <FormLabel id="demo-row-radio-buttons-group-label">
                    Gender
                  </FormLabel>
                  <RadioGroup
                    row
                    aria-labelledby="demo-row-radio-buttons-group-label"
                    name="row-radio-buttons-group"
                  >
                    <FormControlLabel
                      value={1}
                      control={<Radio />}
                      label="Female"
                      {...register("gender", {
                        required: "This field is required",
                      })}
                    />
                    <FormControlLabel
                      value={0}
                      control={<Radio />}
                      label="Male"
                      {...register("gender", {
                        required: "This field is required",
                      })}
                    />
                  </RadioGroup>
                </div>
              </div>
              <p className="error">{errors?.gender?.message}</p>
            </div>
            <div className="wrapper">
              <div className="wrapper2">
                <span>Username </span>
              </div>
              <input
                {...register("username", {
                  required: "This field is required",
                  maxLength: {
                    value: 50,
                    message: "Max length is 50 char.",
                  },
                })}
                placeholder="Username"
              />
              <p className="error">{errors?.username?.message}</p>
            </div>

            <div className="wrapper">
              <div className="wrapper2">
                <span>Password </span>
              </div>
              <input
                {...register("password", {
                  required: "This field is required",
                  maxLength: {
                    value: 20,
                    message: "Max length is 20 char.",
                  },
                })}
                type="password"
                placeholder="Password"
              />
              <p className="error">{errors?.password?.message}</p>
            </div>

            <div className="wrapper">
              <div className="wrapper2">
                <span>Phone Number </span>
              </div>
              <input
                {...register("phoneNumber", {
                  required: "This field is required",
                  pattern: {
                    value:
                      /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/,
                    message: "Invalid Phone Number",
                  },
                })}
                placeholder="Phone Number"
              />
              <p className="error">{errors?.phone?.message}</p>
            </div>

            <div className="wrapper">
              <div className="wrapper2">
                <span>Address </span>
              </div>
              <input
                {...register("streetAddress", {
                  required: "This field is required",
                })}
                placeholder="Address"
              />
              <p className="error">{errors?.address?.message}</p>
            </div>
            <div className="createBtn">
              <button>Create Staff</button>
            </div>
          </Form>
        </FormContainer>
      </MainContainer>
    </Container>
  );
}

export default StaffCreate;
