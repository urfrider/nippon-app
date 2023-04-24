import React, { useEffect, useState } from "react";
import DonutChart from "../../components/DonutChart";
import GraphChart from "../../components/GraphChart";
import Navbar from "../../components/Navbar";
import Sidebar from "../../components/Sidebar";
import BarChart from "../../components/BarChart";
import Widget from "../../components/Widget";
import { useRecoilValue } from "recoil";
import { authAtom } from "../../atoms";
import axios from "axios";
import Loader from "../../components/Loader";
import {
  Container,
  MainContainer,
  Tabs,
} from "../../components/Styled-Containers";

const Dashboard = () => {
  const auth = useRecoilValue(authAtom);
  const [isLoading, setIsLoading] = useState(true);
  const [gpsDataChart, setGpsDataChart] = useState();
  const [transportChart, setTransportChart] = useState();
  const [responseChart, setResponseChart] = useState();

  // to format the number to month in string
  function getMonthName(monthNumber) {
    const date = new Date();
    date.setMonth(monthNumber - 1);
    return date.toLocaleString("en-US", { month: "long" });
  }
  // get gps data for chart
  const getGpsDataChart = async () => {
    try {
      const data = await axios.get(
        ` https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Metadata/GetGPSDataChart`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setGpsDataChart(data.data);
    } catch (Error) {
      console.log(Error);
    }
  };
  // get transport data for chart
  const getTransportChart = async () => {
    try {
      const data = await axios.get(
        ` https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Metadata/GetPublicTransportCount`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setTransportChart(data.data);
      console.log(data.data);
    } catch (Error) {
      console.log(Error);
    }
  };
  // get number of gps logs for chart
  const getResponseChart = async () => {
    try {
      const data = await axios.get(
        ` https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Metadata/GetResponsesDataChart`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setResponseChart(data.data);
    } catch (Error) {
      console.log(Error);
    }
  };
  // get profile
  const getProfile = async () => {
    try {
      const response = await axios.get(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Profile/GetProfile`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      localStorage.setItem("profile", JSON.stringify(response));
    } catch (Error) {
      console.log(Error);
    }
  };
  // get all these data at once
  const getData = async () => {
    await Promise.all([
      getProfile(),
      getGpsDataChart(),
      getTransportChart(),
      getResponseChart(),
    ]);
    setIsLoading(false);
  };
  // get data when screen loaded
  useEffect(() => {
    getData();
  }, []);
  // rerender when loading completed
  useEffect(() => {}, [isLoading]);
  return (
    <Container>
      <Sidebar />
      <MainContainer>
        {isLoading ? (
          <Loader />
        ) : (
          <>
            <Navbar title="DASHBOARD" />
            <Tabs>
              <Widget type="survey_complete" />
              <Widget type="survey-management" />
              <Widget type="gps_logs" />
              <Widget type="user" />
            </Tabs>
            <Tabs>
              <DonutChart
                title="Public Transport Users"
                data={transportChart?.map((item) => item.quantity)}
                labels={transportChart?.map((item) =>
                  item.publicTransport ? "Yes" : "No"
                )}
              />
              <BarChart
                title="Last 4 Months (Survey Responses)"
                data={responseChart?.map((item) => item.count)}
                labels={responseChart?.map((item) =>
                  getMonthName(item.month).slice(0, 3)
                )}
              />
            </Tabs>
            <Tabs>
              <GraphChart
                title="Last 4 Months (GPS Logs)"
                data={gpsDataChart}
              />
            </Tabs>
          </>
        )}
      </MainContainer>
    </Container>
  );
};

export default Dashboard;
