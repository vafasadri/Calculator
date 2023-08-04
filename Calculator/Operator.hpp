#pragma once
#include <cmath>
#include <vector>
#include <string>
#include "GC.h"
#include "UsingMathParser.hpp"

extern GC<Operator> opGC;
using opHandle = Operator*;
extern opHandle
power, multiply, divide, add, subtract, /*greaterequal,smallerEqual,
smaller, greater, equal, nonequal,*/ root, /*in,*/ functionCall,bracket;

