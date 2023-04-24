import React, { useState } from "react";
import Sidebar from "../../components/Sidebar";
import styled from "styled-components";
import Navbar from "../../components/Navbar";
import { useForm } from "react-hook-form";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import FormControl from "@mui/material/FormControl";
import Select from "@mui/material/Select";
import AddIcon from "@mui/icons-material/Add";
import IconButton from "@mui/material/IconButton";
import CloseIcon from "@mui/icons-material/Close";
import RemoveIcon from "@mui/icons-material/Remove";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import {
  Container,
  MainContainer,
  Form,
  Error,
} from "../../components/Styled-Containers";

const MainWrapper = styled(MainContainer)`
  position: relative;
  .addBtn {
    position: sticky;
    margin-left: 20px;
    width: 70px;
    height: 70px;
    display: flex;
    justify-content: center;
    align-items: center;
    top: 100px;
    right: 50px;
    border-radius: 50%;
    background-color: ${(props) => props.theme.accentColor};
    &:hover {
      background-color: #fdcb6e;
      color: black;
      transition: ease-in-out 0.3s;
    }
  }
`;
const Wrapper = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  .wrapper2 {
    flex-direction: column;
  }
`;
const QnsContainer = styled.div`
  position: relative;
  width: 75%;
  min-height: 300px;
  margin-top: 20px;
  background-color: ${(props) => props.theme.bgColor};
  box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -webkit-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -moz-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  border-radius: 30px;
  padding: 2rem;
  h1 {
    color: ${(props) => props.theme.accentColor};
    margin-bottom: 2rem;
    font-weight: bold;
    font-size: 1.5rem;
  }
  .iconBtn {
    position: absolute;
    top: 20px;
    right: 20px;
  }
  input {
    margin-left: 5px;
    width: 60%;
    height: 2rem;
  }
`;

function CreateSurvey() {
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm();
  const navigate = useNavigate();
  const [questions, setQuestions] = useState([
    { question0: "", options: [" "], type: "" },
  ]);
  const [noOfQns, setNoOfQns] = useState(1);

  // handle change of question type
  const handleChange = (event, index) => {
    setQuestions((old) => {
      const temp = [...old];
      temp[index].type = event.target.value;
      return temp;
    });
  };
  // add question
  const addQns = () => {
    setNoOfQns((prev) => prev + 1);
    setQuestions((old) => {
      var temp = "question" + noOfQns;
      var tempObj = { [temp]: "", options: [" "], type: "" };
      var tempArray = [...old];
      tempArray.push(tempObj);
      return tempArray;
    });
  };
  // delete question
  const delQns = (e) => {
    setQuestions((old) => {
      const temp = [...old];
      const optLength = temp[e.currentTarget.id].options.length;
      temp.splice(e.currentTarget.id, 1);
      setValue(`question${e.currentTarget.id}`, "");
      for (var i = 0; i < optLength; i++) {
        setValue(`options${e.currentTarget.id}-${i}`, "");
      }
      return temp;
    });
    setNoOfQns((prev) => prev - 1);
  };

  // create survey on button clicked
  const handleValid = async (data) => {
    const transformedData = {
      Title: data.title,
      Points: data.points,
      Description: data.description,
      Latitude: String(data.latitude),
      Longitude: String(data.longtitude),
      Radius: data.radius,
      Cooldown: data.cooldown,
      Questions: [],
    };

    // transform the data into correct json format to post to backend
    for (let i = 0; i < noOfQns; i++) {
      transformedData.Questions[i] = {
        Question: data[`question${i}`],
        Type: data[`type${i}`],
      };
      if (
        transformedData.Questions[i].Type === 1 ||
        transformedData.Questions[i].Type === 2
      ) {
        transformedData.Questions[i].options = [];
        for (let j = 0; j < 8; j++) {
          if (
            data[`options${i}-${j}`] === undefined ||
            data[`options${i}-${j}`] === ""
          )
            break;
          transformedData.Questions[i].options[j] = data[`options${i}-${j}`];
        }
      } else if (transformedData.Questions[i].Type === 3) {
        transformedData.Questions[i].options = [];
        transformedData.Questions[i].options[0] = "Very\nUnsatisfied";
        transformedData.Questions[i].options[1] = "Unsatisfied\n";
        transformedData.Questions[i].options[2] = "Neutral\n";
        transformedData.Questions[i].options[3] = "Satisfied\n";
        transformedData.Questions[i].options[4] = "Very\nSatisfied";
      }
    }

    //getting user token
    var token = JSON.parse(localStorage.getItem("user")).data;

    const surveyCreationEndpoint =
      "https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/SurveyManagement/CreateSurvey";

    // sending survey creation data to DB
    try {
      await axios.post(surveyCreationEndpoint, transformedData, {
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
          Authorization: "bearer " + token,
        },
      });
      navigate("/survey-management");
    } catch (Error) {
      console.log(Error);
    }
  };
  // add option to a question
  const addOptions = (index) => {
    console.log(index);
    setQuestions((old) => {
      const temp = [...old];
      if (temp[index].options.length >= 8) {
        return temp;
      }
      temp[index].options.push(" ");
      return temp;
    });
  };
  // remove option to a question
  const removeOptions = (index) => {
    console.log(index);
    setQuestions((old) => {
      const temp = [...old];
      if (temp[index].options.length <= 1) {
        return temp;
      }
      temp[index].options.pop();
      return temp;
    });
  };

  return (
    <Container>
      <Sidebar />
      <MainWrapper>
        <Navbar />
        <div className="addBtn">
          <IconButton onClick={addQns} className="iconButton">
            <AddIcon sx={{ color: "white", width: "50px", height: "50px" }} />
          </IconButton>
        </div>
        <Form onSubmit={handleSubmit(handleValid)}>
          <div className="wrapper">
            <div className="idTitle">
              <div className="wrapper2" style={{ marginLeft: "20px" }}>
                <div>
                  <span>Title: </span>
                  <input
                    {...register("title", {
                      required: "This field is required",
                    })}
                    placeholder="Title"
                  />
                </div>
                <Error style={{ marginTop: "-20px" }}>
                  <p>{errors?.title?.message}</p>
                </Error>
              </div>
              <div className="wrapper2">
                <div>
                  <span>Points: </span>
                  <input
                    {...register("points", {
                      required: "This field is required",
                      valueAsNumber: true,
                      validate: (value) => value > 0,
                    })}
                    type="number"
                    min="0"
                    placeholder="Points"
                  />
                </div>
                <Error style={{ marginTop: "-20px" }}>
                  <p>{errors?.points?.message}</p>
                </Error>
              </div>
            </div>

            <div className="idTitle">
              <div className="wrapper2" style={{ marginLeft: "20px" }}>
                <div>
                  <span>Latitude: </span>
                  <input
                    {...register("latitude", {
                      required: "This field is required",
                      validate: (value) => value > 0,
                    })}
                    type="number"
                    step="0.00000000000000001"
                    placeholder="Latitude"
                  />
                </div>
                <Error style={{ marginTop: "-20px" }}>
                  <p>{errors?.title?.message}</p>
                </Error>
              </div>
              <div className="wrapper2">
                <div>
                  <span>Longtitude: </span>
                  <input
                    {...register("longtitude", {
                      required: "This field is required",
                      validate: (value) => value > 0,
                    })}
                    type="number"
                    step="0.00000000000000001"
                    min="0"
                    placeholder="longitude"
                  />
                </div>
                <Error style={{ marginTop: "-20px" }}>
                  <p>{errors?.points?.message}</p>
                </Error>
              </div>
              <div className="wrapper2">
                <div>
                  <span>Radius: </span>
                  <input
                    {...register("radius", {
                      required: "This field is required",
                      valueAsNumber: true,
                      validate: (value) => value > 0,
                    })}
                    placeholder="in meters"
                  />
                </div>
                <Error style={{ marginTop: "-20px" }}>
                  <p>{errors?.title?.message}</p>
                </Error>
              </div>
              <div className="wrapper2">
                <div>
                  <span>Cooldown: </span>
                  <input
                    {...register("cooldown", {
                      required: "This field is required",
                      valueAsNumber: true,
                      validate: (value) => value > 0,
                    })}
                    placeholder="in days"
                  />
                </div>
                <Error style={{ marginTop: "-20px" }}>
                  <p>{errors?.title?.message}</p>
                </Error>
              </div>
            </div>

            <div className="description">
              <span>Description: </span>
              <textarea
                placeholder="Description"
                {...register("description", {
                  required: "This field is required",
                })}
              />
            </div>
            <Error style={{ marginTop: "-20px", marginBottom: "20px" }}>
              <p>{errors?.points?.message}</p>
            </Error>
          </div>
          {questions?.map((question, index) => (
            <Wrapper key={index}>
              <QnsContainer>
                <h1>question {index + 1}</h1>
                <div className="iconBtn">
                  <IconButton id={index} onClick={delQns}>
                    <CloseIcon />
                  </IconButton>
                </div>
                <div>
                  <input
                    {...register(`question${index}`, {
                      required: "This field is required",
                    })}
                    placeholder="Ask a question"
                  />

                  <p
                    style={{
                      color: "red",
                      marginTop: "10px",
                      marginLeft: "5px",
                    }}
                  >
                    {errors?.["question" + index]?.message}
                  </p>
                </div>
                <div>
                  {question.type === 0 ||
                  question.type === "" ||
                  question.type === 3 ? (
                    []
                  ) : (
                    <>
                      {question.options.map((option, index2) => (
                        <div key={index2}>
                          <input
                            {...register(`options${index}-${index2}`, {
                              required: "This field is required",
                            })}
                            placeholder={`Option ${index2 + 1}`}
                          />
                          <IconButton onClick={() => addOptions(index)}>
                            <AddIcon />
                          </IconButton>
                          <IconButton onClick={() => removeOptions(index)}>
                            <RemoveIcon />
                          </IconButton>
                        </div>
                      ))}
                    </>
                  )}
                </div>
                <FormControl
                  sx={{ m: 1, minWidth: 120, height: 50 }}
                  size="small"
                >
                  <InputLabel id="demo-select-small">Type</InputLabel>
                  <Select
                    {...register(`type${index}`, {
                      required: "This field is required",
                    })}
                    labelId="demo-select-small"
                    id="demo-select-small"
                    value={question.type}
                    label="Type"
                    onChange={(event) => handleChange(event, index)}
                  >
                    <MenuItem value="">
                      <em>None</em>
                    </MenuItem>
                    <MenuItem value={0}>Open-Ended</MenuItem>
                    <MenuItem value={1}>Radio</MenuItem>
                    <MenuItem value={2}>Multi-Option</MenuItem>
                    <MenuItem value={3}>Likert</MenuItem>
                  </Select>
                </FormControl>
              </QnsContainer>
            </Wrapper>
          ))}
          <div className="submitBtnDiv">
            <button className="submitBtn">Create Survey</button>
          </div>
        </Form>
      </MainWrapper>
    </Container>
  );
}

export default CreateSurvey;
