import Navbar from "../../components/Navbar";
import Sidebar from "../../components/Sidebar";
import DataTable from "../../components/DataTable";
import React, { useEffect, useState } from "react";
import AddIcon from "@mui/icons-material/Add";
import IconButton from "@mui/material/IconButton";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { authAtom } from "../../atoms";
import { useRecoilValue } from "recoil";
import ConfirmDialog from "../../components/ConfirmDialog";
import Loader from "../../components/Loader";
import {
  Container,
  MainContainer,
  DatatableWrapper,
  Header,
} from "../../components/Styled-Containers";

function Rewards() {
  const profile = JSON.parse(window.localStorage.getItem("profile"));
  const auth = useRecoilValue(authAtom);
  const [rewards, setRewards] = useState();
  const [isLoading, setIsLoading] = useState(true);
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
  // navigate to create reward
  const navigate = useNavigate();
  const onClick = () => {
    navigate("/rewards/create");
  };
  // get rewards data
  const getRewards = async () => {
    try {
      const rewardsData = await axios.get(
        "https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Reward/AllRewards",
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setRewards(rewardsData.data);
    } catch (Error) {
      console.log(Error);
    } finally {
      setIsLoading(false);
    }
  };
  // set rows for datatable
  const rewardRow = rewards?.map((reward, index) => ({
    no: index + 1,
    id: reward.rewardId,
    rewardname: reward.rewardName,
    description: reward.rewardDesc,
    points: reward.rewardPoints,
    quantity: reward.rewardQty,
  }));
  // set columns for datatable
  const columns = [
    { field: "no", headerName: "NO", width: 70 },
    {
      field: "rewardname",
      headerName: "Title",
    },
    {
      field: "description",
      headerName: "Description",
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
      field: "quantity",
      headerName: "Quantity",
      width: 100,
    },
    {
      field: "action",
      headerName: "Action",
      // render delete button
      renderCell: (params) => {
        return (
          <div className="cellAction">
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
      const response = await axios.delete(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Reward/${id}?deletedBy=Hi`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      getRewards();
      console.log(response);
    } catch (Error) {
      console.log(Error);
    }
  };

  useEffect(() => {
    getRewards();
  }, []);

  return (
    <Container>
      <Sidebar />
      <MainContainer>
        <Navbar title="REWARDS" />
        <Header>
          <h1>Reward List</h1>
          <IconButton onClick={onClick} className="iconButton">
            <AddIcon sx={{ color: "white", width: "40px", height: "40px" }} />
          </IconButton>
        </Header>
        <DatatableWrapper>
          {isLoading ? (
            <Loader />
          ) : (
            <DataTable rows={rewardRow} columns={columns} />
          )}
        </DatatableWrapper>
      </MainContainer>
      <ConfirmDialog
        confirmDialog={confirmDialog}
        setConfirmDialog={setConfirmDialog}
      />
    </Container>
  );
}

export default Rewards;
