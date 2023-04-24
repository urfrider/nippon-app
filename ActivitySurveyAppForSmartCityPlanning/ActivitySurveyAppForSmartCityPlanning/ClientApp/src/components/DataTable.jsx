import React from "react";
import { DataGrid } from "@mui/x-data-grid";
import styled from "styled-components";

const Table = styled.div`
  box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -webkit-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -moz-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  border-radius: 10px;
  height: 600px;
  min-width: 80vw;
  width: 100%;
  padding: 20px;
  background-color: ${(props) => props.theme.bgColor};
  .cellWithStatus {
    padding: 5px;
    border-radius: 5px;
    &.active,
    &.available {
      color: green;
      background-color: rgba(0, 128, 0, 0.05);
    }
    &.offline {
      color: crimson;
      background-color: rgba(255, 0, 0, 0.05);
    }
    &.admin {
      color: green;
      background-color: rgba(0, 128, 0, 0.05);
    }
    &.user {
      color: crimson;
      background-color: rgba(0, 128, 0, 0.05);
    }
    &.points {
      color: #e776bc;
      font-weight: 600;
      background-color: rgba(226, 24, 237, 0.05);
    }
  }
  .cellAction {
    display: flex;
    align-items: center;
    gap: 15px;
    .viewButton {
      padding: 2px 5px;
      border-radius: 5px;
      color: darkblue;
      border: 1px dotted rgba(0, 0, 139, 0.596);
      cursor: pointer;
    }
    .deleteButton {
      padding: 2px 5px;
      border-radius: 5px;
      color: crimson;
      border: 1px dotted rgba(220, 20, 60, 0.6);
      cursor: pointer;
    }
    .resultButton {
      padding: 2px 5px;
      border-radius: 5px;
      color: goldenrod;
      border: 1px dotted rgba(107, 99, 15, 0.733);
      cursor: pointer;
    }
  }
  button {
    border: none;
    background-color: transparent;
  }
`;

function Datatable({ columns, rows, role }) {
  return (
    <Table>
      {rows && columns && (
        <DataGrid
          rows={rows}
          columns={columns}
          pageSize={9}
          rowsPerPageOptions={[9]}
          checkboxSelection
        />
      )}
    </Table>
  );
}

export default Datatable;
