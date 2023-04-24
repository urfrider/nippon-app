import mapboxgl from "!mapbox-gl"; // eslint-disable-line import/no-webpack-loader-syntax
import styled from "styled-components";
import "mapbox-gl/dist/mapbox-gl.css";
import React, { useEffect, useState } from "react";
import axios from "axios";
import { authAtom } from "../atoms";
import { useRecoilValue } from "recoil";
import Button from "@mui/material/Button";
import ButtonGroup from "@mui/material/ButtonGroup";
import Box from "@mui/material/Box";
import ReactDOM from "react-dom/client";
import mrt from "../static/publicTransportData/mrt.json";
import TrainIcon from "@mui/icons-material/Train";
import AirportShuttleIcon from "@mui/icons-material/AirportShuttle";
import bus from "../static/publicTransportData/bus.json";
import Loader from "./Loader";
import { LoaderContainer } from "./Styled-Containers";

// access to mapbox
mapboxgl.accessToken = process.env.REACT_APP_MAPBOX_API_KEY;

const Mapbox = styled.div`
  height: 100%;
  width: 100%;
`;

const MapLoaderContainer = styled(LoaderContainer)`
  .hide {
    opacity: 0;
  }
`;

const MapContainer = styled.div`
  height: 90%;
  width: 90%;
  display: flex;
  align-items: center;
  justify-content: center;
  background: ${(props) => props.theme.bgColor};
  border-radius: 10px;
  box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -webkit-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -moz-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  margin-bottom: -10px;
`;

const CSVButton = styled.a`
  color: white;
  background-color: ${(props) => props.theme.accentColor};
  padding: 15px;
  border-radius: 15px;
  text-decoration: none;
  &:hover {
    text-decoration: none;
    color: white;
    background-color: #fdcb6e;
  }
`;

const Pointer = styled.button`
  width: 25px;
  height: 25px;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: transparent;
  border: none;
`;

const BtnContainer = styled.div`
  display: flex;
  button {
    padding: 10px;
  }
  .mrt {
    border-radius: 15px 0px 0px 15px;
  }
  .bus {
    border-radius: 0px 15px 15px 0px;
  }
`;

const ButtonWrapper = styled.div`
  width: 100%;
  margin-bottom: 30px;
  display: flex;
  flex-direction: row;
  justify-content: space-around;
  align-items: center;
`;

// Bus / MRT marker
const Marker = ({ onClick, type, feature }) => {
  const _onClick = () => {
    onClick(feature.properties.title);
  };

  return (
    <Pointer onClick={_onClick} className="marker">
      {type === "mrt" ? (
        <TrainIcon sx={{ fontSize: "small" }} />
      ) : (
        <AirportShuttleIcon sx={{ fontSize: "small" }} />
      )}
    </Pointer>
  );
};

const markerClicked = (title) => {
  window.alert(title);
};

const Map = () => {
  const mapContainerRef = React.useRef(null);
  const [location, setLocation] = useState();
  const [loading, setLoading] = useState(true);
  const auth = useRecoilValue(authAtom);
  const [mrtToggle, setMrtToggle] = useState(false);
  const [busToggle, setBusToggle] = useState(false);

  const toggleMrt = () => setMrtToggle((prev) => !prev);
  const toggleBus = () => setBusToggle((prev) => !prev);

  // get the location data of specific date
  const onClickDuration = async (noDays) => {
    // format the date to sent to the backend
    var endDateObj = new Date();
    var endMonth = endDateObj.getUTCMonth() + 1; //months from 1-12
    var endDay = endDateObj.getUTCDate();
    var endYear = endDateObj.getUTCFullYear();
    var endDate = endDateObj.getTime();
    var startDate = endDate - noDays * 24 * 60 * 60 * 1000;
    var startDateObj = new Date(startDate);
    var startMonth = startDateObj.getUTCMonth() + 1; //months from 1-12
    var startDay = startDateObj.getUTCDate();
    var startYear = startDateObj.getUTCFullYear();
    const dateData = {
      tartDate_Day: startDay,
      startDate_Month: startMonth,
      startDate_Year: startYear,
      endDate_Day: endDay,
      endDate_Month: endMonth,
      endDate_Year: endYear,
    };
    console.log(dateData);
    try {
      const locationData = await axios.post(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/heatmap`,
        dateData,
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setLocation(locationData.data);
    } catch (Error) {
      console.log(Error);
    }
  };
  // get initial location data
  const get_locationData = async () => {
    var dateObj = new Date();
    var month = dateObj.getUTCMonth() + 1; //months from 1-12
    var day = dateObj.getUTCDate();
    var year = dateObj.getUTCFullYear();
    const dateData = {
      startDate_Day: 11,
      startDate_Month: 12,
      startDate_Year: 2022,
      endDate_Day: day,
      endDate_Month: month,
      endDate_Year: year,
    };
    try {
      const locationData = await axios.post(
        `https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/heatmap`,
        {
          ...dateData,
        },
        {
          headers: {
            Authorization: "bearer " + auth.data,
          },
        }
      );
      setLocation(locationData.data);
      setLoading(false);
    } catch (Error) {
      console.log(Error);
    }
  };

  useEffect(() => {
    get_locationData();
  }, []);

  useEffect(() => {
    // initialise the map location
    const map = new mapboxgl.Map({
      container: mapContainerRef.current,
      style: "mapbox://styles/mapbox/streets-v11",
      center: [103.8198, 1.3521],
      zoom: 11,
    });
    // when loading completed
    if (!loading) {
      if (mrtToggle) {
        // Render custom marker components
        mrt.features.forEach((feature) => {
          // Create a React ref
          const ref = React.createRef();
          // Create a new DOM node and save it to the React ref
          ref.current = document.createElement("div");
          // Render a Marker Component on our new DOM node
          const root = ReactDOM.createRoot(ref.current);
          root.render(
            <Marker type="mrt" onClick={markerClicked} feature={feature} />
          );

          // Create a Mapbox Marker at our new DOM node
          new mapboxgl.Marker(ref.current)
            .setLngLat(feature.geometry.coordinates)
            .addTo(map);
        });
      }

      if (busToggle) {
        bus.features.forEach((feature) => {
          // Create a React ref
          const ref = React.createRef();
          // Create a new DOM node and save it to the React ref
          ref.current = document.createElement("div");
          // Render a Marker Component on our new DOM node
          const root = ReactDOM.createRoot(ref.current);
          root.render(
            <Marker type={bus} onClick={markerClicked} feature={feature} />
          );

          // Create a Mapbox Marker at our new DOM node
          new mapboxgl.Marker(ref.current)
            .setLngLat(feature.geometry.coordinates)
            .addTo(map);
        });
      }
      // initialise the heatmap
      map.on("load", () => {
        map.addSource("trees", {
          type: "geojson",
          data: location,
        });
        map.addLayer(
          {
            id: "trees-heat",
            type: "heatmap",
            source: "trees",
            maxzoom: 15,
            paint: {
              // increase weight as diameter breast height increases
              "heatmap-weight": {
                property: "dbh",
                type: "exponential",
                stops: [
                  [1, 0],
                  [62, 1],
                ],
              },
              // increase intensity as zoom level increases
              "heatmap-intensity": {
                stops: [
                  [11, 1],
                  [15, 3],
                ],
              },
              // assign color values be applied to points depending on their density
              "heatmap-color": [
                "interpolate",
                ["linear"],
                ["heatmap-density"],
                0,
                "rgba(236,222,239,0)",
                0.2,
                "rgb(208,209,230)",
                0.4,
                "rgb(166,189,219)",
                0.6,
                "rgb(103,169,207)",
                0.8,
                "rgb(28,144,153)",
              ],
              // increase radius as zoom increases
              "heatmap-radius": {
                stops: [
                  [11, 15],
                  [15, 20],
                ],
              },
              // decrease opacity to transition into the circle layer
              "heatmap-opacity": {
                default: 1,
                stops: [
                  [14, 1],
                  [15, 0],
                ],
              },
            },
          },
          "waterway-label"
        );
      });
    }
    return () => map.remove();
  }, [location, busToggle, mrtToggle]);

  return (
    <>
      {loading ? (
        <MapLoaderContainer>
          <Loader />
          <div className="hide" ref={mapContainerRef}></div>
        </MapLoaderContainer>
      ) : (
        <>
          <ButtonWrapper>
            <BtnContainer>
              <button className="mrt" onClick={toggleMrt}>
                <TrainIcon />
              </button>
              <button className="bus" onClick={toggleBus}>
                <AirportShuttleIcon />
              </button>
            </BtnContainer>
            <ButtonGroup variant="outlined" aria-label="outlined button group">
              <Button onClick={() => onClickDuration(25)}>Last 1 month</Button>
              <Button onClick={() => onClickDuration(90)}>Last 3 months</Button>
              <Button onClick={() => onClickDuration(180)}>
                Last 6 months
              </Button>
            </ButtonGroup>
            <CSVButton href="https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Metadata/ExportAllGps">
              Download CSV
            </CSVButton>
          </ButtonWrapper>
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
              "& > *": {
                m: 1,
              },
            }}
          ></Box>
          <MapContainer>
            <Mapbox ref={mapContainerRef} className="map" />
          </MapContainer>
        </>
      )}
    </>
  );
};

export default Map;
