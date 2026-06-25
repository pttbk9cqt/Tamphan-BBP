    function BoldAndAddSpace(input) {
                                let value = input.value.replace(/\s/g, '');
                                input.value = value.split('').join(' ');
                              }