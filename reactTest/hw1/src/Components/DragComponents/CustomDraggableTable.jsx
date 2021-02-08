import React, { useEffect, useState } from "react";
import { DragDropContext, Draggable, Droppable } from "react-beautiful-dnd";
import { MyNavbar } from "../MyNavbar";
import { DraggableTableColumn } from "./DraggableTableColumn";
import { Button, Spinner, Container, Row, Col } from "reactstrap";
import { AddDeveloperModal } from "../AddDeveloperModal";
import { DraggableButtonsColumn } from "./DraggableButtonsColumn";
import { ViewDeveloperModal } from "../ViewDeveloperModal";
import { EditDeveloperModal } from "../EditDeveloperModal";

export const CustomDraggableTable = () => {
  const [columnNames, setColumnNames] = useState([]);

  useEffect(() => {
    setColumnNames((names) => [...names, "firstName"]);
    setColumnNames((names) => [...names, "lastName"]);
    setColumnNames((names) => [...names, "age"]);
  }, []);

  const [developers, setDevelopers] = useState([])
  const [selectedDeveloper, setSelectedDeveloper] = useState({})
  const [follows, setFollows] = useState([])

  const [isModalOpen, setIsModalOpen] = useState(false)
  const [isSpinnerHidden, setIsSpinnerHidden] = useState(true)

  const [viewModalIsOpen, setViewModalIsOpen] = useState(false)
  const [editModalIsOpen, setEditModalIsOpen] = useState(false)

  useEffect(()=>{
    setDevelopers(devs=>[...devs, {firstName:"Ibrahim", lastName:"Huseynzade", age:19}])
    setDevelopers(devs=>[...devs, {firstName:"Ibra", lastName:"Kadabra", age:24}])
  }, [])

  const viewModalToggle = () =>{
    setViewModalIsOpen(vm=>!vm)
}

const editModalToggle = () =>{
  setEditModalIsOpen(em=>!em)
}

const viewButtonClick = (index) =>{
    setSelectedDeveloper(developers[index])
    console.log(index)
    console.log(developers[index])
    viewModalToggle()
} 

const editButtonClick = (index) =>{
  setSelectedDeveloper({...developers[index],id:index})
  console.log(index)
  console.log(developers[index])
  editModalToggle()
}

const editDeveloperSubmit = async(event) => {
  let developer = {}
  developer.firstName = event.target.firstName.value
  developer.lastName = event.target.lastName.value
  developer.age = event.target.age.value
  let id = event.target.id.value

  if(developer.firstName !== '' && developer.lastName !== '' && developer.age > 0 && developer.age < 100) 
  {
    editDeveloper(developer,id);
    editModalToggle()
  }
  event.preventDefault()
}

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

  const dragEndHandler = (result) => {
      try{
          if(result.destination.droppableId == 'remove')
          console.log('Remove mE!!')
          let newColumns = []
          columnNames.forEach(name => {
              console.log(name + '  ', result.draggableId)
              if(name!=result.draggableId) newColumns.push(name);
          });
          setColumnNames(newColumns)
          console.log(newColumns)
        } catch {}
        console.log(result)
  };

  return (
    <div>
      <MyNavbar follows={follows}/>
      <ViewDeveloperModal developer={selectedDeveloper} isModalOpen={viewModalIsOpen} toggleModal={viewModalToggle} />
      <EditDeveloperModal developer={selectedDeveloper} isModalOpen={editModalIsOpen} toggleModal={editModalToggle} submitCallback={editDeveloperSubmit}/>
      <AddDeveloperModal submitCallback={addDeveloperSubmit} isModalOpen={isModalOpen} toggleModal={addDeveloperButtonClick}/>
      <DragDropContext onDragEnd={dragEndHandler}>
        <Container className="App">
            <Row>
                <Col xs='10'>
                <Droppable droppableId="test" direction='horizontal'>
                    {(provided) => (
                        <div style={{display:'flex'}} {...provided.droppableProps} ref={provided.innerRef}>
                            {columnNames.map((name, i) => (
                                <Col>
                                    <DraggableTableColumn columnName={name} index={i} developers={developers}/>
                                </Col>
                            ))}
                            <Col>
                                <DraggableButtonsColumn developers={developers} followDeveloperCallback={followDeveloper} viewButtonClick={viewButtonClick} editButtonClick={editButtonClick}/>
                            </Col>
                        {provided.placeholder}
                        </div>
                    )}
                </Droppable>
                    </Col>
                    <Col xs='2'>
                <Droppable droppableId="remove" direction='horizontal'>
                    {(provided) => (
                        <div style={{display:'flex', border:'solid 3px', backgroundColor:'#f54e42', height:'400px'}} {...provided.droppableProps} ref={provided.innerRef}>
                           <h1 style={{position:'absolute'}}>remove Column</h1>
                        {provided.placeholder}
                        </div>
                    )}
                </Droppable>
                    </Col>
            </Row>
            <Spinner className="mb-5" hidden={isSpinnerHidden} color="primary" />
            <Row>
              <Button color="danger" onClick={addDeveloperButtonClick}>
                Add Developer
              </Button>
            </Row>
        </Container>
      </DragDropContext>
    </div>
  );
};
