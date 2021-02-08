import React from 'react'
import { Button } from 'reactstrap';

export const DraggableButtonsColumn = ({developers, followDeveloperCallback, viewButtonClick, editButtonClick, i}) => {
    return (
        <div>            
            <h1>Buttons</h1>
            {developers.map((devs,i) => (
                <div key={i} style={{display:'flex'}}>
                    <Button color="outline-danger" className="mr-2" onClick={()=>followDeveloperCallback(i)}>Follow</Button>
                    <Button color="outline-primary" className="mr-2" onClick={()=>viewButtonClick(i)}>View</Button>
                    <Button color="outline-success" onClick={()=>editButtonClick(i)}>Edit</Button>
                </div>
            ))}
        </div>
    )
}
