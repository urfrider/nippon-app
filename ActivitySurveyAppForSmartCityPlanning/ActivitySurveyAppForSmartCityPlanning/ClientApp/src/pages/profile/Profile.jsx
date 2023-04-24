import React from "react";
import Navbar from "../../components/Navbar";
import Sidebar from "../../components/Sidebar";
import {
  Container,
  MainContainer,
  ProfileContainer,
} from "../../components/Styled-Containers";

function Profile() {
  const profile = JSON.parse(window.localStorage.getItem("profile"));

  return (
    <Container>
      <Sidebar />
      <MainContainer>
        <Navbar />
        <ProfileContainer>
          <div className="left">
            <h1>Information</h1>
            <div className="item">
              <img
                className="itemImg"
                src="https://icons.veryicon.com/png/o/internet--web/55-common-web-icons/person-4.png"
              />
              <div className="details">
                <h1 className="itemTitle">{profile?.data?.username}</h1>
                <div className="detailItem">
                  <span className="itemKey">Role:</span>
                  <span className="itemValue">
                    {profile?.data?.role === 2 ? "Admin" : "Dashboard User"}
                  </span>
                </div>
              </div>
            </div>
          </div>
        </ProfileContainer>
      </MainContainer>
    </Container>
  );
}

export default Profile;
