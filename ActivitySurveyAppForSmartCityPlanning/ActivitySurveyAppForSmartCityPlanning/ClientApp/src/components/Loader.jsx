import * as React from "react";
import CircularProgress from "@mui/material/CircularProgress";
import Box from "@mui/material/Box";
import { LoaderContainer } from "./Styled-Containers";

export default function Loader() {
  return (
    <LoaderContainer>
      <Box sx={{ display: "flex" }}>
        <CircularProgress />
      </Box>
    </LoaderContainer>
  );
}
