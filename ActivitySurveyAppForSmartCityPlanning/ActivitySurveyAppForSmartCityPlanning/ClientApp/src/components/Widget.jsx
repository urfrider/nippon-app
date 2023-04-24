import React, { useState } from "react";
import styled from "styled-components";
import PersonOutlineIcon from "@mui/icons-material/PersonOutline";
import axios from "axios";
import { Link } from "react-router-dom";
import NoteAltIcon from "@mui/icons-material/NoteAlt";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import { useEffect } from "react";
import { authAtom } from "../atoms";
import { useRecoilValue } from "recoil";
import Loader from "./Loader";

const Container = styled.div`
  display: flex;
  flex: 1;
  padding: 10px;
  height: 100px;
  color: ${(props) => props.theme.contentColor};
  background: ${(props) => props.theme.bgColor};
  border-radius: 10px;
  justify-content: space-between;
  box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -webkit-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -moz-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  .left,
  .right {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    .title {
      font-size: 1rem;
      font-weight: bold;
      margin-bottom: 10px;
      color: ${(props) => props.theme.accentColor};
    }
    .counter {
      font-size: 1.6rem;
    }
    .link {
      border-bottom: 1px solid grey;
      cursor: pointer;
    }
    .icon {
      font-size: 2rem;
      padding: 5px;
      border-radius: 5px;
      align-self: flex-end;
    }
  }
`;

function Widget({ type }) {
  const [surveysCompleted, setSurveysCompleted] = useState();
  const [survey, setSurvey] = useState();
  const [gps, setGps] = useState();
  const [user, setUser] = useState();
  const [loading, setLoading] = useState(true);
  const auth = useRecoilValue(authAtom);

  // get number of surveys completed
  async function getSurveyCompleted() {
    try {
      const data = await axios.get(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Metadata/TotalResponse`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setSurveysCompleted(data.data);
    } catch (Error) {
      console.log(Error);
    }
  }
  // get number of surveys created
  const getSurvey = async () => {
    try {
      const data = await axios.get(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Metadata/TotalSurvey`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setSurvey(data.data);
    } catch (Error) {
      console.log(Error);
    }
  };
  // get number of gps logs
  const getGps = async () => {
    try {
      const data = await axios.get(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Metadata/TotalGps`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setGps(data.data);
    } catch (Error) {
      console.log(Error);
    }
  };
  // get number of mobile users
  const getUsers = async () => {
    try {
      const data = await axios.get(
        ` https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Metadata/TotalAccount`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setUser(data.data);
    } catch (Error) {
      console.log(Error);
    }
  };
  // get all data at once
  const getData = async () => {
    await Promise.all([
      getSurveyCompleted(),
      getSurvey(),
      getGps(),
      getUsers(),
    ]);
    setLoading(false);
  };

  useEffect(() => {
    getData();
  }, []);

  var data;
  // differentiate widget types
  switch (type) {
    case "survey_complete":
      data = {
        title: "Surveys Completed",
        counter: surveysCompleted,
        icon: (
          <NoteAltIcon
            className="icon"
            style={{
              color: "green",
              backgroundColor: "rgba(218, 165, 32, 0.2)",
            }}
          />
        ),
        type: "users",
      };
      break;
    case "gps_logs":
      data = {
        title: "GPS Logs",
        counter: gps,
        icon: (
          <LocationOnIcon
            className="icon"
            style={{
              backgroundColor: "rgba(218, 165, 32, 0.2)",
              color: "goldenrod",
            }}
          />
        ),
        type: "customers",
      };
      break;
    case "survey-management":
      data = {
        title: "Survey Sets",
        counter: survey,
        link: "See all surveys",
        icon: (
          <NoteAltIcon
            className="icon"
            style={{
              backgroundColor: "rgba(218, 165, 32, 0.2)",
              color: "goldenrod",
            }}
          />
        ),
        type: "survey-management",
      };
      break;
    case "user":
      data = {
        title: "Users Registered",
        counter: user,
        link: "See all users",
        icon: (
          <PersonOutlineIcon
            className="icon"
            style={{
              backgroundColor: "rgba(218, 165, 32, 0.2)",
              color: "goldenrod",
            }}
          />
        ),
        type: "user",
      };
    default:
      break;
  }
  return loading ? (
    <Loader />
  ) : (
    <Container>
      <div className="left">
        <span className="title">{data?.title}</span>
        <span className="counter">{data?.counter}</span>
        <Link to={`/${data?.type}`} state={{ type: data?.type }}>
          <span className="link">{data?.link}</span>
        </Link>
      </div>
      <div className="right">{data?.icon}</div>
    </Container>
  );
}

export default Widget;
