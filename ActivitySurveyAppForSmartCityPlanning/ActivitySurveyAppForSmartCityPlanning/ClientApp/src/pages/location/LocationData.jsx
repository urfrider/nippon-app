import React from "react";
import styled from "styled-components";
import Navbar from "../../components/Navbar";
import Sidebar from "../../components/Sidebar";
import Map from "../../components/Map";
import {
  Container,
  MainContainer,
  Tabs,
} from "../../components/Styled-Containers";

const MapContainer = styled(Tabs)`
  gap: 0px;
  height: 85%;
  width: 100%;
  flex-direction: column;
  justify-content: center;
  align-items: center;
`;

function LocationData() {
  return (
    <Container>
      <Sidebar />
      <MainContainer>
        <Navbar title="LOCATION DATA" />
        <MapContainer>
          <Map />
        </MapContainer>
      </MainContainer>
    </Container>
  );
}

export default LocationData;
