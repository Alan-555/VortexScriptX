template? Animal { //template? marks this template as abstract
    int legCount, //required
    string name, //required
    bool alive = true, //default
    bool vaccinated = false, //default
    //dollar works as 'var' declare either like this: int x = 1 or $x = 2 and the type is inferred
    $IsVaccinated, //final
     IsVaccinated = string (){// /- marks an expression that is multi-line and each line is treated as a bracket (highest precedence)
        return /-name+" is "+ 
                 if this.vaccinated: "" else "not "+
                 "vaccinated"
    } //TODO: syntax sugar for same-class finalization
}

template? Quadrupled from Animal {
    legCount = 4, //final
}

template? Pet from Quadrupled {
    default vaccinated  = true //set new default
    object owner
    default name = owner.ToString()+"'s "+this.template.name

}

template Dog from Pet;

$dog = Dog{
    owner = "Mark"
}

dog.name //Mark's Dog