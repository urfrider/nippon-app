import React from "react";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import styled from "styled-components";
import { ChartContainer, ChartTitle } from "./Styled-Containers";

const Container = styled(ChartContainer)`
  height: fit-content;
`;

const Title = styled.span`
  font-weight: bold;
`;

function ResponseTable({ title, data }) {
  const rows = data?.map((d, index) => ({ id: index + 1, response: d }));

  return (
    <Container>
      <ChartTitle>
        <h1>{title}</h1>
      </ChartTitle>
      <TableContainer component={Paper}>
        <Table sx={{ minWidth: 650 }} aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell align="center">
                <Title>No</Title>
              </TableCell>
              <TableCell align="center">
                <Title>Response</Title>
              </TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {rows.map((row) => (
              <TableRow key={row.id}>
                <TableCell align="center">{row.id}</TableCell>
                <TableCell align="center">{row.response}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  );
}

export default ResponseTable;
