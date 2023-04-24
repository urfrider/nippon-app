import React, { useState } from "react";
import { useParams } from "react-router-dom";
import Navbar from "../../components/Navbar";
import Sidebar from "../../components/Sidebar";
import { useEffect } from "react";
import axios from "axios";
import { authAtom } from "../../atoms";
import { useRecoilValue } from "recoil";
import {
  Container,
  MainContainer,
  ProfileContainer,
} from "../../components/Styled-Containers";

function UserView() {
  const { userId } = useParams();
  const auth = useRecoilValue(authAtom);
  const [user, setUser] = useState();
  // get user data
  async function fetchUser() {
    try {
      const response = await axios.get(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/AccountDetail/${userId}`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setUser(response.data);
    } catch (Error) {
      console.log(Error);
    }
  }

  useEffect(() => {
    fetchUser();
  }, []);

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
                <h1 className="itemTitle">{user?.accDetailsFirstName}</h1>
                <div className="detailItem">
                  <span className="itemKey">Phone:</span>
                  <span className="itemValue">
                    +{user?.accDetailsPhoneCountryCode}{" "}
                    {user?.accDetailsPhoneNumber}
                  </span>
                </div>
                <div className="detailItem">
                  <span className="itemKey">Address:</span>
                  <span className="itemValue">
                    {user?.accDetailsAddressStreet}
                  </span>
                </div>
                <div className="detailItem">
                  <span className="itemKey">Country:</span>
                  <span className="itemValue">
                    {user?.accDetailsAddressCountry}
                  </span>
                </div>
                <div className="detailItem">
                  <span className="itemKey">Points:</span>
                  <span className="itemValue">
                    {user?.accDetailsTotalPoints}
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

export default UserView;
