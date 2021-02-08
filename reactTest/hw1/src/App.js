import logo from './logo.svg';
import './App.css';
import { useEffect, useState } from 'react';
import { Button, Spinner, Container, Row, Col } from 'reactstrap';
import { DeveloperTable } from './Components/DeveloperTable';
import { AddDeveloperModal } from './Components/AddDeveloperModal';
import { MyNavbar } from './Components/MyNavbar';
import { DndProvider } from 'react-dnd';
import { HTML5Backend } from 'react-dnd-html5-backend';

function App() {

  const [developers, setDevelopers] = useState([])
  const [follows, setFollows] = useState([])

  const [isModalOpen, setIsModalOpen] = useState(false)
  const [isSpinnerHidden, setIsSpinnerHidden] = useState(true)

  useEffect(()=>{
    setDevelopers(devs=>[...devs, {firstName:"Ibrahim", lastName:"Huseynzade", age:19}])
    setDevelopers(devs=>[...devs, {firstName:"Ibra", lastName:"Kadabra", age:24}])
  }, [])

  const editDeveloper = (developer, id) => {

    let newDevelopers = []
    developers.forEach((dev, i) => {
      if(i == id) newDevelopers.push(developer);
      else newDevelopers.push(dev);
    });
    setDevelopers(newDevelopers)
  }

  const addDeveloperButtonClick = () => {
    setIsModalOpen(t=>!t)
  }

  const addDeveloperSubmit = async(event) => {
    let developer = {}
    developer.firstName = event.target.firstName.value
    developer.lastName = event.target.lastName.value
    developer.age = event.target.age.value

    setIsSpinnerHidden(false)
    setTimeout(()=> {
      setIsSpinnerHidden(true)
      console.log("finished")
  
      setDevelopers(devs=>devs=[...developers, developer])
    } ,1000)
    addDeveloperButtonClick()
    event.preventDefault()
  }

  const followDeveloper = (index) => {
    let developer = developers[index]

    let exist = false;
    follows.forEach(dev => {
      if(dev.firstName === developer.firstName && dev.lastName === developer.lastName && dev.age === developer.age)
      {
        exist = true;
        return;
      }
    });
    if(exist == true) return;

    setFollows(f=>[...f, developer])
  }

  return (
    <>
    <MyNavbar follows={follows}/>
    <Container className="App">
      <Row>
        <Col>
          <DeveloperTable developers={developers} editDeveloperCallback={editDeveloper} followDeveloperCallback={followDeveloper}></DeveloperTable>
          <AddDeveloperModal submitCallback={addDeveloperSubmit} isModalOpen={isModalOpen} toggleModal={addDeveloperButtonClick}/>
          <Spinner className="mb-5" hidden={isSpinnerHidden} color="primary" />
          <Button color="danger" onClick={addDeveloperButtonClick}>Add Developer</Button>
        </Col>
      </Row>
    </Container>
    </>
  );
}

export default App;
