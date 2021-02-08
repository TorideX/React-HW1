import React, { useState } from "react";
import { Button, Table } from "reactstrap"
import { ViewDeveloperModal } from "./ViewDeveloperModal";
import { EditDeveloperModal } from "./EditDeveloperModal";

export const DeveloperTable = ({ developers, editDeveloperCallback, followDeveloperCallback }) => {

  const [selectedDeveloper, setSelectedDeveloper] = useState({})
  const [viewModalIsOpen, setViewModalIsOpen] = useState(false)
  const [editModalIsOpen, setEditModalIsOpen] = useState(false)

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
      editDeveloperCallback(developer,id);
      editModalToggle()
    }
    event.preventDefault()
  }

  return (
    <div>        
      <ViewDeveloperModal developer={selectedDeveloper} isModalOpen={viewModalIsOpen} toggleModal={viewModalToggle} />
      <EditDeveloperModal developer={selectedDeveloper} isModalOpen={editModalIsOpen} toggleModal={editModalToggle} submitCallback={editDeveloperSubmit}/>
      <Table>
        <thead>
          <tr>
            <th>#</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Age</th>
            <th>Buttons</th>
          </tr>
        </thead>
        <tbody>
          {
              developers.map((d,i)=>(
                <tr key={i} onDoubleClick={()=>viewButtonClick(i)}>
                    <th scope="row">{i + 1}</th>
                    <td>{d.firstName}</td>
                    <td>{d.lastName}</td>
                    <td>{d.age}</td>
                    <td>
                        <Button color="outline-danger" className="mr-2" onClick={()=>followDeveloperCallback(i)}>Follow</Button>
                        <Button color="outline-primary" className="mr-2" onClick={()=>viewButtonClick(i)}>View</Button>
                        <Button color="outline-success" onClick={()=>editButtonClick(i)}>Edit</Button>
                    </td>
                </tr>
              ))
          }
        </tbody>
      </Table>
    </div>
  );
};
