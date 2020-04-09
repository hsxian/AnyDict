#!/bin/bash

_any_dict()
{
    cmd='./publish/AnyDict.Args -p '$2
    $cmd
    
    dir='/tmp/any-dict/prompt/'
    file=$dir$1
    prompts=`cat $file`
    COMPREPLY=()
    cur=${COMP_WORDS[COMP_CWORD]}
    COMPREPLY=($(compgen -W "${prompts[*]}" -- $cur))
}

complete -F _any_dict ad 