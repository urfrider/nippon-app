import React, { useState } from "react";
import DataTable from "../../components/DataTable";
import Navbar from "../../components/Navbar";
import Sidebar from "../../components/Sidebar";
import { useNavigate } from "react-router-dom";
import AddIcon from "@mui/icons-material/Add";
import IconButton from "@mui/material/IconButton";
import axios from "axios";
import { authAtom } from "../../atoms";
import { useRecoilValue } from "recoil";
import { Link } from "react-router-dom";
import Loader from "../../components/Loader";
import ConfirmDialog from "../../components/ConfirmDialog";
import { useQuery, useQueryClient } from "react-query";
import {
  Container,
  MainContainer,
  DatatableWrapper,
  Header,
} from "../../components/Styled-Containers";

function Staff() {
  const queryClient = useQueryClient();
  const profile = JSON.parse(window.localStorage.getItem("profile"));
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

  const { isLoading: staffLoading, data: staffData } = useQuery(
    "allStaff",
    fetchStaff
  );
  // get staff data
  async function fetchStaff() {
    try {
      const staffResponse = await axios.get(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Account/GetAllDashboardUsers`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      const adminResponse = await axios.get(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Account/GetAllAdmins`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      const response = staffResponse.data.concat(adminResponse.data);
      return response;
    } catch (Error) {
      console.log(Error);
    }
  }
  // set rows for datatable
  const staffRows = staffData?.map((staff, index) => ({
    no: index + 1,
    id: staff.accId,
    fullData: staff,
    staffname: staff.accUsername,
    role: staff.accRole === 2 ? "admin" : "user",
  }));
  // set columns for datatable
  const columns = [
    { field: "no", headerName: "NO", width: 70 },
    {
      field: "staffname",
      headerName: "Staffname",
      width: 230,
    },
    {
      field: "role",
      headerName: "Role",
      width: 200,
      renderCell: (params) => {
        return (
          <div className={`cellWithStatus ${params.row.role}`}>
            {params.row.role}
          </div>
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
            <Link to={`/staff/${params.row.id}`} state={params.row.fullData}>
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
      const response = await axios.delete(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Account/${id}?deletedBy=Hi`,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      await queryClient.refetchQueries(["allStaff"]);
      console.log(response);
    } catch (Error) {
      console.log(Error);
    }
  };
  // navigate to staff create page
  const onClick = () => {
    navigate("/staff/create");
  };

  return (
    <Container>
      <Sidebar />
      <MainContainer>
        <Navbar title={"ACCOUNTS"} />
        <Header>
          <h1>Staff List</h1>
          <IconButton onClick={onClick} className="iconButton">
            <AddIcon sx={{ color: "white", width: "40px", height: "40px" }} />
          </IconButton>
        </Header>
        <DatatableWrapper>
          {staffLoading ? (
            <Loader />
          ) : (
            <DataTable rows={staffRows} columns={columns} />
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

export default Staff;
