import React, { useEffect, useState } from "react";
import DataTable from "../../components/DataTable";
import Navbar from "../../components/Navbar";
import Sidebar from "../../components/Sidebar";
import AddIcon from "@mui/icons-material/Add";
import IconButton from "@mui/material/IconButton";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { authAtom } from "../../atoms";
import { useRecoilValue } from "recoil";
import { Link } from "react-router-dom";
import ConfirmDialog from "../../components/ConfirmDialog";
import {
  Container,
  MainContainer,
  DatatableWrapper,
  Header,
} from "../../components/Styled-Containers";
import Button from "react-bootstrap/Button";

const SurveyManagement = () => {
  const profile = JSON.parse(window.localStorage.getItem("profile"));
  const [surveys, setSurveys] = useState();
  const navigate = useNavigate();
  const auth = useRecoilValue(authAtom);
  const [confirmDialog, setConfirmDialog] = useState({
    isOpen: false,
    title: "",
    subtitle: "",
    onConfirm: () => {},
  });
  // close dialog on delete
  const onDeleteClick = (id) => {
    setConfirmDialog({
      isOpen: true,
      title: "Delete",
      subtitle: "Are you sure to delete?",
      onConfirm: () => {
        deleteRow(id);
        setConfirmDialog({ ...confirmDialog, isOpen: false });
      },
    });
  };
  // get survey data
  const get_survey = async () => {
    try {
      const surveyDatas = await axios.get(
        "https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Survey",
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setSurveys(surveyDatas.data);
      console.log(surveyDatas);
    } catch (Error) {
      console.log(Error);
    }
  };
  // clean up survey data for each row
  const surveyData = surveys?.map((survey, index) => ({
    no: index + 1,
    id: survey.surveyId,
    title: survey.surveyTitle,
    status: survey.surveyDisable === false ? "active" : "offline",
    response: survey.numResponses,
    points: survey.surveyPoints,
  }));
  // set rows for datatable
  const surveyRows = surveyData?.map((survey) => ({
    no: survey.no,
    id: survey.id,
    title: survey.title,
    status: survey.status,
    response: survey.response,
    points: survey.points,
  }));
  // set columns for datatable
  const columns = [
    { field: "no", headerName: "NO", width: 70 },
    {
      field: "title",
      headerName: "Title",
      width: 150,
      renderCell: (params) => {
        return <div>{params.row.title}</div>;
      },
    },
    {
      field: "status",
      headerName: "Status",
      width: 100,
      renderCell: (params) => {
        return (
          <div className={`cellWithStatus ${params.row.status}`}>
            {params.row.status}
          </div>
        );
      },
    },
    {
      field: "response",
      headerName: "Responses",
      width: 100,
    },
    {
      field: "points",
      headerName: "Points",
      width: 100,
      renderCell: (params) => {
        return (
          <div className={`cellWithStatus points`}>{params.row.points}</div>
        );
      },
    },
    {
      field: "action",
      headerName: "Action",
      width: 200,
      renderCell: (params) => {
        return (
          <div className="cellAction">
            <Link to={`/survey-management/result/${params.row.id}`}>
              <div className="resultButton">Result</div>
            </Link>
            <Link to={`/survey-management/${params.row.id}`}>
              <div className="viewButton">View</div>
            </Link>
            {profile?.data?.role === 2 && (
              <button onClick={() => onDeleteClick(params.row.id)}>
                <div className="deleteButton">Delete</div>
              </button>
            )}
          </div>
        );
      },
    },
  ];
  // delete a row
  const deleteRow = async (id) => {
    try {
      await axios.delete(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Survey/${id}?deletedBy=Hi`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      get_survey();
    } catch (Error) {
      console.log(Error);
    }
  };
  // navigate to survey create page
  const onClick = () => {
    navigate("/survey-management/create");
  };

  useEffect(() => {
    get_survey();
  }, []);

  const triggerSurvey = async () => {
    try {
      await axios.get(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/SurveyTrigger/CheckTriggers`
      );
    } catch (Error) {
      console.log(Error);
    }
  };

  return (
    <Container>
      <Sidebar />
      <MainContainer>
        <Navbar title="SURVEY MANAGEMENT" />
        <Header>
          <h1>Survey List</h1>
          {/* <Button variant="primary" onClick={triggerSurvey}>Trigger Survey</Button> */}
          <IconButton onClick={onClick} className="iconButton">
            <AddIcon sx={{ color: "white", width: "40px", height: "40px" }} />
          </IconButton>
        </Header>
        <DatatableWrapper>
          {<DataTable rows={surveyRows} columns={columns} role={2} />}
        </DatatableWrapper>
      </MainContainer>
      <ConfirmDialog
        confirmDialog={confirmDialog}
        setConfirmDialog={setConfirmDialog}
      />
    </Container>
  );
};

export default SurveyManagement;
