//lang def

template Object{

    ...

 Template template

}

//user
template? Animal{
    string name,
    number legCount,
    void() doAction 
}

template? Quadruped extends Animal{
    legCount = 4
}

Dog extends Quadruped{
    string race,
    doAction = ():
        print("Woof")
    ;

}




$dog = new Dog{
    name = "Spotty"
    race = "German Shepard"
    legCount = 1 //error!

    template = Dog //automatically inferred
}


dog.legCount //4