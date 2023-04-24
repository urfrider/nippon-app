import React from "react";
import Sidebar from "../../components/Sidebar";
import Navbar from "../../components/Navbar";
import { useForm } from "react-hook-form";
import { authAtom } from "../../atoms";
import { useRecoilValue } from "recoil";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import {
  Container,
  MainContainer,
  Form,
  Error,
} from "../../components/Styled-Containers";

function CreateReward() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();
  const auth = useRecoilValue(authAtom);
  const navigate = useNavigate();
  // on submit create rewards button
  const handleValid = async (data) => {
    const transformedData = {
      rewardName: data.title,
      rewardPoints: data.points,
      rewardDesc: data.description,
      rewardQty: data.quantity,
    };
    const rewardsCreationEndpoint =
      "https://biqtoc5csc.execute-api.ap-southeast-1.amazonaws.com/api/Reward/CreateReward?createdBy='admin'";

    //sending survey creation data to DB
    try {
      await axios.post(rewardsCreationEndpoint, transformedData, {
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
          Authorization: "bearer " + auth.data,
        },
      });
      navigate("/rewards");
    } catch (Error) {
      console.log(Error);
    }
  };

  return (
    <Container>
      <Sidebar />
      <MainContainer>
        <Navbar />
        <Form onSubmit={handleSubmit(handleValid)}>
          <div className="wrapper">
            <div className="idTitle">
              <div className="wrapper2" style={{ marginLeft: "20px" }}>
                <div>
                  <span>Title: </span>
                  <input
                    {...register("title", {
                      required: "This field is required",
                    })}
                    placeholder="Title"
                  />
                </div>
                <Error style={{ marginTop: "-20px" }}>
                  <p>{errors?.title?.message}</p>
                </Error>
              </div>
              <div className="wrapper2">
                <div>
                  <span>Points: </span>
                  <input
                    {...register("points", {
                      required: "This field is required",
                      valueAsNumber: true,
                      validate: (value) => value > 0,
                    })}
                    type="number"
                    min="0"
                    placeholder="Points"
                  />
                </div>
                <Error style={{ marginTop: "-20px" }}>
                  <p>{errors?.points?.message}</p>
                </Error>
              </div>
              <div className="wrapper2">
                <div>
                  <span>Quantity: </span>
                  <input
                    {...register("quantity", {
                      required: "This field is required",
                    })}
                    type="number"
                    placeholder="Quantity"
                  />
                </div>
                <Error style={{ marginTop: "-20px" }}>
                  <p>{errors?.title?.message}</p>
                </Error>
              </div>
            </div>

            <div className="description">
              <span>Description: </span>
              <textarea
                placeholder="Description"
                {...register("description", {
                  required: "This field is required",
                })}
              />
            </div>
            <Error style={{ marginTop: "-20px", marginBottom: "20px" }}>
              <p>{errors?.points?.message}</p>
            </Error>
          </div>

          <div className="submitBtnDiv">
            <button className="submitBtn">Create Rewards</button>
          </div>
        </Form>
      </MainContainer>
    </Container>
  );
}

export default CreateReward;
