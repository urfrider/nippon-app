import React from "react";
import ApexChart from "react-apexcharts";
import { ChartContainer, ChartTitle } from "./Styled-Containers";
import styled from "styled-components";

const Wrapper = styled(ChartContainer)`
  height: fit-content;
`;

function GraphChart({ title, data }) {
  // format number to month in string
  function getMonthName(monthNumber) {
    if (monthNumber === 2) {
      return "February";
    }
    const date = new Date();
    date.setMonth(monthNumber - 1);
    return date.toLocaleString("en-US", { month: "long" });
  }

  return (
    <Wrapper>
      <ChartTitle>
        <h1>{title}</h1>
      </ChartTitle>
      <ApexChart
        type="area"
        height="100%"
        width="100%"
        series={[
          {
            name: "Total response",
            data: data?.map((item) => item.count),
          },
        ]}
        options={{
          xaxis: {
            categories: data?.map((item) =>
              getMonthName(item.month).slice(0, 3)
            ),
          },
          colors: ["#7d5fff"],
        }}
      />
    </Wrapper>
  );
}

export default GraphChart;
