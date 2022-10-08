#include "BruteForce.h"

BruteForce::BruteForce(int** graph, int source)
{
    this->graph = graph;
    this->source = source;
}

BruteForce::~BruteForce()
{
    delete this->graph;
}
