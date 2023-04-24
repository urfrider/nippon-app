import React from "react";
import styled from "styled-components";
import { useSetRecoilState } from "recoil";
import { isDarkAtom } from "../atoms";
import DarkModeOutlinedIcon from "@mui/icons-material/DarkModeOutlined";
import LogoutIcon from "@mui/icons-material/Logout";
import IconButton from "@mui/material/IconButton";
import { useNavigate } from "react-router-dom";
import { authAtom } from "../atoms";

const Container = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-bottom: 0.5px solid rgb(231, 228, 228);
  background-color: ${(props) => props.theme.bgColor};
  height: 80px;
  padding: 20px;
`;

const Header = styled.div`
  font-size: 1.5rem;
  font-weight: bold;
  color: ${(props) => props.theme.accentColor};
`;

const Items = styled.div`
  display: flex;
  align-items: center;
`;

const Item = styled.div`
  display: flex;
  align-items: center;
  margin-right: 20px;
  flex-direction: column;
  .icon {
    color: ${(props) => props.theme.accentColor};
  }
  .welcome {
    font-size: 1rem;
    color: ${(props) => props.theme.accentColor};
    margin-bottom: 0.5rem;
  }
  .username {
    font-size: 1.1rem;
    color: ${(props) => props.theme.accentColor};
    opacity: 0.8;
    font-weight: 600;
  }
`;

function Navbar({ title }) {
  const profile = JSON.parse(window.localStorage.getItem("profile"));
  const setDarkAtom = useSetRecoilState(isDarkAtom);
  const setAuth = useSetRecoilState(authAtom);
  const toggleDarkAtom = () => setDarkAtom((prev) => !prev);
  const navigate = useNavigate();
  const logout = () => {
    localStorage.removeItem("profile");
    localStorage.removeItem("user");
    setAuth(null);
    navigate("/");
  };

  return (
    <Container>
      <Header>{title}</Header>
      <Items>
        <Item>
          <DarkModeOutlinedIcon
            onClick={toggleDarkAtom}
            className="darkModeIcon icon"
          />
        </Item>
        <Item>
          <IconButton onClick={logout}>
            <LogoutIcon className="icon" />
          </IconButton>
        </Item>
        <Item>
          <span className="welcome">Welcome!</span>
          <span className="username">{profile?.data?.username}</span>
        </Item>
      </Items>
    </Container>
  );
}

export default React.memo(Navbar);
