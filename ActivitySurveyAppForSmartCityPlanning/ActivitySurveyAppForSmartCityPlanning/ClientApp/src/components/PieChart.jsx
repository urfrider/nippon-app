import React from "react";
import ApexChart from "react-apexcharts";
import { ChartContainer, ChartTitle } from "./Styled-Containers";

function PieChart({ title, labels, data }) {
  return (
    <ChartContainer>
      <ChartTitle>
        <h1>{title}</h1>
      </ChartTitle>
      <ApexChart
        className="chart"
        type="pie"
        height="100%"
        width="100%"
        series={data}
        options={{
          chart: {
            width: 500,
            type: "pie",
          },
          stroke: {
            colors: ["transparent"],
          },
          plotOptions: {
            pie: {
              customScale: 0.9,
              dataLabels: {
                offset: -10,
              },
            },
          },
          labels: labels,
          dataLabels: {
            style: {
              fontSize: "11px",
              colors: ["white"],
            },
          },
          legend: {
            formatter: function (seriesName, opts) {
              return (
                seriesName + ":  " + opts.w.globals.series[opts.seriesIndex]
              );
            },
          },
          theme: {
            mode: "light",
            palette: "palette1",
          },
          colors: [
            "#eb5f1a",
            "#2a54a1",
            "#f6a417",
            "#34b3e7",
            "#008FFB",
            "#e3b017",
            "#5bb1c7",
          ],
        }}
      />
    </ChartContainer>
  );
}

export default PieChart;
