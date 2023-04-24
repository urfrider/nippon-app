import React from "react";
import styled from "styled-components";
import DashboardIcon from "@mui/icons-material/Dashboard";
import AccountBoxIcon from "@mui/icons-material/AccountBox";
import SupervisedUserCircleIcon from "@mui/icons-material/SupervisedUserCircle";
import PeopleOutlineIcon from "@mui/icons-material/PeopleOutline";
import PollIcon from "@mui/icons-material/Poll";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import { Link } from "react-router-dom";
import Logo from "../static/images/login-logo.png";
import RedeemIcon from "@mui/icons-material/Redeem";

const Container = styled.div`
  flex: 1;
  border-right: 0.5px solid rgb(230, 227, 227);
  min-height: 100vh;
  background-color: ${(props) => props.theme.accentColor};
`;
const Top = styled.div`
  height: 80px;
  background-color: white;
  a {
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
  }
  img {
    padding: 0.5rem;
    width: 80%;
  }
`;
const Center = styled.div`
  margin-left: 10px;
  ul {
    list-style: none;
    p {
      margin-top: 15px;
      margin-bottom: 10px;
      font-size: 1rem;
      text-transform: uppercase;
      font-weight: bold;
      color: ${(props) => props.theme.textColor};
    }
    li {
      display: flex;
      align-items: center;
      padding: 5px;
      margin-bottom: 10px;
      cursor: pointer;
      &:hover {
        background-color: #80a3f0;
      }
      span {
        font-size: 0.9rem;
        margin-left: 10px;
        font-weight: 400;
        color: ${(props) => props.theme.textColor};
      }
      .icon {
        font-size: 1.5rem;
        color: ${(props) => props.theme.textColor};
      }
    }
    a {
      text-decoration: none;
      color: inherit;
    }
  }
`;

function Sidebar() {
  return (
    <Container>
      <Top>
        <Link to="/dashboard">
          <img src={Logo} />
        </Link>
      </Top>
      <Center>
        <ul>
          <p>Menu</p>
          <Link to="/dashboard">
            <li>
              <DashboardIcon className="icon" />
              <span>Home</span>
            </li>
          </Link>
          <Link to={"/survey-management"} state={{ type: "users" }}>
            <li>
              <PollIcon className="icon" />
              <span>Survey Management</span>
            </li>
          </Link>
          <Link to={"/location-data"}>
            <li>
              <LocationOnIcon className="icon" />
              <span>Location Data</span>
            </li>
          </Link>
          <p style={{ marginTop: "30px" }}>Accounts</p>
          <Link to={"/staff"} state={{ type: "staff" }}>
            <li>
              <SupervisedUserCircleIcon className="icon" />
              <span>Staffs</span>
            </li>
          </Link>
          <Link to={"/user"} state={{ type: "users" }}>
            <li>
              <PeopleOutlineIcon className="icon" />
              <span>Users</span>
            </li>
          </Link>
          <p style={{ marginTop: "30px" }}>Services</p>
          <Link to={"/rewards"} state={{ type: "rewards" }}>
            <li>
              <RedeemIcon className="icon" />
              <span>Rewards</span>
            </li>
          </Link>
          <Link to={"/profile"}>
            <li>
              <AccountBoxIcon className="icon" />
              <span>Profile</span>
            </li>
          </Link>
        </ul>
      </Center>
    </Container>
  );
}

export default Sidebar;
