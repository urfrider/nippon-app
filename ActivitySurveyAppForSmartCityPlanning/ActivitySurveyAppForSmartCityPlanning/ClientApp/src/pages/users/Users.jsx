import React, { useState } from "react";
import DataTable from "../../components/DataTable";
import Navbar from "../../components/Navbar";
import Sidebar from "../../components/Sidebar";
import axios from "axios";
import { authAtom } from "../../atoms";
import { useRecoilValue } from "recoil";
import { useQuery, useQueryClient } from "react-query";
import { Link } from "react-router-dom";
import Loader from "../../components/Loader";
import ConfirmDialog from "../../components/ConfirmDialog";
import {
  Container,
  MainContainer,
  DatatableWrapper,
  Header,
} from "../../components/Styled-Containers";

function Users() {
  const queryClient = useQueryClient();
  const { isLoading: userLoading, data: userData } = useQuery(
    "allUser",
    fetchUsers
  );
  const profile = JSON.parse(window.localStorage.getItem("profile"));
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
  // get user data
  async function fetchUsers() {
    try {
      const response = await axios.get(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Account/GetAllMobileUsers`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      return response.data;
    } catch (Error) {
      console.log(Error);
    }
  }
  // set rows for datatable
  const userRows = userData?.map((user, index) => ({
    no: index + 1,
    id: user.accId,
    username: user.accUsername,
    points: user.accDetailsTotalPoints,
  }));
  // set columns for datatable
  const columns = [
    { field: "no", headerName: "NO", width: 70 },
    {
      field: "username",
      headerName: "Username",
      width: 230,
    },
    {
      field: "points",
      headerName: "Points",
      width: 200,
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
            <Link to={`/user/${params.row.id}`}>
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
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Account/${id}?deletedBy=Hi`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      await queryClient.refetchQueries(["allUser"]);
    } catch (Error) {
      console.log(Error);
    }
  };

  return (
    <Container>
      <Sidebar />
      <MainContainer>
        <Navbar title={"ACCOUNTS"} />
        <DatatableWrapper>
          <Header>
            <h1>User List</h1>
          </Header>
          {userLoading ? (
            <Loader />
          ) : (
            <DataTable rows={userRows} columns={columns} />
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

export default Users;
