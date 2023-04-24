import styled from "styled-components";

const Container = styled.div`
  display: flex;
  background-color: ${(props) => props.theme.homeBgColor};
`;
const MainContainer = styled.div`
  flex: 6;
`;
const Tabs = styled.div`
  display: flex;
  gap: 20px;
  padding: 20px;
`;
const Header = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.25rem;
  margin: -0.5rem 0.5rem;
  h1 {
    color: ${(props) => props.theme.accentColor};
    font-size: 1.2rem;
    font-weight: bold;
    padding: 10px;
  }
  .iconButton {
    background-color: ${(props) => props.theme.accentColor};
    &:hover {
      background-color: #fdcb6e;
    }
  }
`;
const DatatableWrapper = styled(Tabs)`
  flex-direction: column;
  justify-content: center;
`;
const Form = styled.form`
  padding: 2rem;
  gap: 10px;
  span {
    color: black;
  }
  div {
    margin-bottom: 1rem;
  }
  .submitBtn {
    width: 30%;
    padding: 20px;
    border-radius: 30px;
    color: white;
    border: none;
    background-color: ${(props) => props.theme.accentColor};
    &:hover {
      background-color: #fdcb6e;
      color: black;
      transition: ease-in-out 0.3s;
    }
  }
  .submitBtnDiv {
    display: flex;
    justify-content: center;
    align-items: center;
    margin-top: 30px;
  }
  .wrapper {
    margin: auto;
    width: 60%;
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    background-color: #ffeaa7;
    box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
    -webkit-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
    -moz-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
    border-radius: 10px;
    span {
      margin-right: 10px;
    }
    .description {
      display: flex;
      width: 60%;
      align-items: flex-start;
      margin-right: 20px;
      textarea {
        width: 100%;
        height: 80px;
      }
    }
  }
  .idTitle {
    width: 80%;
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 10px;
    padding: 20px;
    margin-bottom: -10px;
    input {
      width: 80%;
      height: 30px;
      background-color: transparent;
      margin-top: 8px;
      border: none;
      border-bottom: 2px solid black;
      font-size: 1.1rem;
      &:focus {
        outline: none;
        border-bottom: 2px solid white;
        background-color: inherit;
      }
    }
  }
`;
const Error = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  p {
    justify-content: flex-start;
    color: red;
    margin-top: 10px;
  }
`;
const ProfileContainer = styled(Tabs)`
  color: ${(props) => props.theme.contentColor};
  .left {
    position: relative;
    flex: 1;
    -webkit-box-shadow: 2px 4px 10px 1px rgba(0, 0, 0, 0.47);
    box-shadow: 2px 4px 10px 1px rgba(201, 201, 201, 0.47);
    padding: 20px;
    border-radius: 10px;
    background-color: ${(props) => props.theme.bgColor};
    h1 {
      margin-bottom: 20px;
      font-weight: bold;
      color: ${(props) => props.theme.accentColor};
    }
    .item {
      display: flex;
      gap: 20px;
      .itemImg {
        height: 100px;
        width: 100px;
        border-radius: 50%;
        object-fit: cover;
      }
    }
    .details {
      .itemTitle {
        margin-bottom: 10px;
        color: #555;
      }
      .detailItem {
        margin-bottom: 10px;
        font-size: 14px;
        .itemKey {
          font-weight: bold;
          color: gray;
          margin-right: 5px;
        }
        .itemValue {
          font-weight: 300;
        }
      }
    }
  }
`;
const ChartContainer = styled.div`
  flex: 2;
  padding: 20px;
  height: 300px;
  color: ${(props) => props.theme.contentColor};
  background: ${(props) => props.theme.bgColor};
  border-radius: 10px;
  flex-direction: column;
  box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -webkit-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
  -moz-box-shadow: 1px 16px 19px -7px rgba(135, 125, 125, 0.71);
`;
const ChartTitle = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  color: ${(props) => props.theme.accentColor};
  margin-bottom: 20px;
  h1 {
    font-size: 1.1rem;
    font-weight: bold;
  }
`;
const LoaderContainer = styled.div`
  display: flex;
  width: 100%;
  height: 100%;
  justify-content: center;
  align-items: center;
`;

export {
  Container,
  MainContainer,
  Tabs,
  DatatableWrapper,
  Header,
  Form,
  Error,
  ProfileContainer,
  ChartContainer,
  ChartTitle,
  LoaderContainer,
};
