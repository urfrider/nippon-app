import React from "react";
import ApexChart from "react-apexcharts";
import { ChartContainer, ChartTitle } from "./Styled-Containers";

function DonutChart({ title, labels, data }) {
  return (
    <ChartContainer>
      <ChartTitle>
        <h1>{title}</h1>
      </ChartTitle>
      <ApexChart
        type="donut"
        height="90%"
        width="100%"
        series={data}
        options={{
          chart: {
            height: 300,
            width: 400,
          },
          labels: labels,
          colors: ["#ff3838", "#17c0eb"],
        }}
      />
    </ChartContainer>
  );
}

export default DonutChart;
