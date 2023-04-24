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

function StaffView() {
    const { staffId } = useParams();
    const auth = useRecoilValue(authAtom);
    const [staff, setStaff] = useState();

    // get staff data
    async function fetchStaff() {
        try {
            const response = await axios.get(
                `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/AccountDetail/${staffId}`,
                //`https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Profile/GetFullProfileDetails/${staffId}`,
                {
                    headers: {
                        Authorization: "bearer " + auth.data,
                    },
                }
            );
            setStaff(response.data);
        } catch (Error) {
            console.log(Error);
        }
    }

    useEffect(() => {
        fetchStaff();
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
                                <h1 className="itemTitle">{staff?.accDetailsFirstName}</h1>
                                <div className="detailItem">
                                    <span className="itemKey">Phone:</span>
                                    <span className="itemValue">
                                        +{staff?.accDetailsPhoneCountryCode}{" "}
                                        {staff?.accDetailsPhoneNumber}
                                    </span>
                                </div>
                                <div className="detailItem">
                                    <span className="itemKey">Address:</span>
                                    <span className="itemValue">
                                        {staff?.accDetailsAddressStreet}
                                    </span>
                                </div>
                                <div className="detailItem">
                                    <span className="itemKey">Country:</span>
                                    <span className="itemValue">
                                        {staff?.accDetailsAddressCountry}
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

export default StaffView;
