import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import styled from "styled-components";
import Navbar from "../../components/Navbar";
import Sidebar from "../../components/Sidebar";
import axios from "axios";
import { authAtom } from "../../atoms";
import { useRecoilValue } from "recoil";
import DonutChart from "../../components/DonutChart";
import BarChart from "../../components/BarChart";
import PieChart from "../../components/PieChart";
import ResponseTable from "../../components/ResponseTable";
import { Container, MainContainer } from "../../components/Styled-Containers";

const Charts = styled.div`
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 20px;
  padding: 10px;
`;

function SurveyResult() {
  const { surveyId } = useParams();
  const [surveyResult, setSurveyResult] = useState();
  console.log(surveyId);
  const auth = useRecoilValue(authAtom);

  // get survey result data
  async function fetchSurveyResult() {
    try {
      const response = await axios.get(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Metadata/GetResponseData/${surveyId}`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setSurveyResult(response.data);
    } catch (Error) {
      console.log(Error);
    }
  }

  useEffect(() => {
    fetchSurveyResult();
  }, []);

  return (
    <Container>
      <Sidebar />
      <MainContainer>
        <Navbar />
        <Charts>
          {surveyResult?.map((question, index) =>
            question.qnsType === 0 ? (
              <ResponseTable
                key={index}
                title={question.qns}
                data={question.opendEndedData}
              />
            ) : question.qnsType === 1 ? (
              <DonutChart
                key={index}
                title={question.qns}
                labels={question.labels}
                data={question.data}
              />
            ) : question.qnsType === 2 ? (
              <BarChart
                key={index}
                title={question.qns}
                labels={question.labels}
                data={question.data}
              />
            ) : question.qnsType === 3 ? (
              <PieChart
                key={index}
                title={question.qns}
                labels={question.labels}
                data={question.data}
              />
            ) : null
          )}
        </Charts>
      </MainContainer>
    </Container>
  );
}

export default SurveyResult;
