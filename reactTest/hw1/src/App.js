import logo from './logo.svg';
import './App.css';
import { useEffect, useState } from 'react';
import { Button } from 'reactstrap';
import { DeveloperTable } from './Components/DeveloperTable';
import { AddDeveloperModal } from './Components/AddDeveloperModal';
import { ViewDeveloperModal } from './Components/ViewDeveloperModal';

function App() {

  const [developers, setDevelopers] = useState([])
  const [isModalOpen, setIsModalOpen] = useState(false)

  useEffect(()=>{
    // developers.push({firstName:"Ibrahim", lastName:"Huseynzade", age:19})
    // developers.push({firstName:"Ibra", lastName:"Kadabra", age:24})
  }, [])

  const viewDeveloperButtonClick = () => {

  }

  const addDeveloperButtonClick = () => {
    setIsModalOpen(t=>t=!t)
  }

  const addDeveloperSubmit = (event) => {
    let developer = {}
    developer.firstName = event.target.firstName.value
    developer.lastName = event.target.lastName.value
    developer.age = event.target.age.value

    setDevelopers(devs=>devs=[...developers, developer])
    console.log(developers)

    addDeveloperButtonClick()
    event.preventDefault()
  }

  return (
    <div className="App">
      <DeveloperTable developers={developers}></DeveloperTable>
      <AddDeveloperModal submitCallback={addDeveloperSubmit} isModalOpen={isModalOpen} toggleModal={addDeveloperButtonClick}/>
      <Button color="danger" onClick={addDeveloperButtonClick}>Add Developer</Button>
    </div>
  );
}

export default App;
