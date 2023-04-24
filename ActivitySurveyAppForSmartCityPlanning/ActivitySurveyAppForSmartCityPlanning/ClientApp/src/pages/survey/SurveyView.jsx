import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import styled from "styled-components";
import Navbar from "../../components/Navbar";
import Sidebar from "../../components/Sidebar";
import axios from "axios";
import { authAtom } from "../../atoms";
import { useRecoilValue } from "recoil";
import QuestionAnswerIcon from "@mui/icons-material/QuestionAnswer";
import { Container, MainContainer } from "../../components/Styled-Containers";

const Wrapper = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
`;

const Attribute = styled.div`
  width: 100%;
  display: flex;
  margin-bottom: 10px;
  span {
    font-size: 1.1rem;
    font-weight: 600;
  }
`;

const Label = styled.span`
  display: flex;
  margin-right: 10px;
  font-size: 1.1rem;
`;

const Box = styled.div`
  position: relative;
  display: flex;
  margin-top: 50px;
  width: 50%;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  background-color: ${(props) => props.theme.bgColor};
  box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -webkit-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -moz-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  border-radius: 30px;
  padding: 2rem;
  color: black;
  &:last-child {
    margin-bottom: 50px;
  }
`;

const Question = styled.span`
  font-size: 1.2rem;
  font-weight: 600;
  margin-bottom: 20px;
  width: 100%;
`;

const Options = styled.span`
  font-size: 1.2rem;
  font-weight: 600;
  margin-bottom: 10px;
  width: 100%;
`;

const Option = styled.span`
  font-size: 1.1rem;
  margin-bottom: 5px;
  font-weight: 400;
  display: flex;
  width: 100%;
  justify-content: flex-start;
`;

const Header = styled.h1`
  font-size: 2rem;
  font-weight: 600;
  color: ${(props) => props.theme.accentColor};
  margin-top: 40px;
  margin-bottom: -20px;
`;

const Type = styled.span`
  padding: 5px;
  border-radius: 5px;
  align-self: flex-end;
  &.open {
    color: green;
    background-color: rgba(0, 128, 0, 0.05);
  }
  &.radio {
    color: crimson;
    background-color: rgba(255, 0, 0, 0.05);
  }
  &.multi {
    color: goldenrod;
    border: 1px dotted rgba(107, 99, 15, 0.733);
  }
  &.likert {
    color: salmon;
    border: 1px dotted rgba(255, 195, 155, 0.733);
  }
`;

function SurveyView() {
  const { surveyId } = useParams();
  const [survey, setSurvey] = useState();
  const auth = useRecoilValue(authAtom);
  // get survey data
  const get_survey = async () => {
    try {
      const surveyData = await axios.get(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/SurveyManagement/GetSurvey/${surveyId}`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setSurvey(surveyData.data);
    } catch (Error) {
      console.log(Error);
    }
  };

  useEffect(() => {
    get_survey();
  }, []);

  return (
    <Container>
      <Sidebar />
      <MainContainer>
        <Navbar />
        <Wrapper>
          <Header>BASIC INFORMATION</Header>
          <Box>
            <div style={{ width: "60%" }}>
              <Attribute>
                <Label>Title: </Label>
                <span>{survey?.title}</span>
              </Attribute>
              <Attribute>
                <Label>Points:</Label>
                <span>{survey?.points}</span>
              </Attribute>
              <Attribute>
                <Label>Description:</Label>
                <span>{survey?.description}</span>
              </Attribute>
            </div>
          </Box>

          <Header>QUESTION LIST</Header>
          {survey?.questions?.map((question, index) => (
            <Box key={index}>
              <QuestionAnswerIcon
                style={{ position: "absolute", top: "20", right: "20" }}
              />
              <Question>
                Question {index + 1}: {question.question}
              </Question>
              {question.options.length > 0 && <Options>Options:</Options>}
              {question.options?.map((option, index) => (
                <Option key={index}>
                  {index + 1}: {option}
                </Option>
              ))}
              {question.type === 0 && <Type className="open">OPEN-ENDED</Type>}
              {question.type === 1 && <Type className="radio">RADIO</Type>}
              {question.type === 2 && (
                <Type className="multi">MULTI-OPTION</Type>
              )}
              {question.type === 3 && <Type className="likert">LIKERT</Type>}
            </Box>
          ))}
        </Wrapper>
      </MainContainer>
    </Container>
  );
}

export default SurveyView;
